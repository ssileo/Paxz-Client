using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kernys.Bson;
using System.Timers;
using System.Collections.Generic;
using BasicTypes;

namespace krak.Utils
{
    public static class Timers
    {

        public static Timer TeleportTimer;

        public static void Load()
        {
            TeleportTimer = new Timer();
            TeleportTimer.Interval = 1500;
            TeleportTimer.Elapsed += onTeleportTimer;
            TeleportTimer.AutoReset = true;
            TeleportTimer.Enabled = true;
        }

        public static void onTeleportTimer(System.Object source, System.Timers.ElapsedEventArgs e)
        {
            if (!KrakMonoBehaviour.teleport.isTeleporting) return;

            KrakMonoBehaviour.teleport.sentTicks++;

            if (KrakMonoBehaviour.teleport.sentTicks > 2)
            {
                KrakMonoBehaviour.teleport.sentTicks = 0;
                return;
            }

            if (KrakMonoBehaviour.teleport.fromSpawn && !KrakMonoBehaviour.teleport.fromSpawnSent)
            {
                KrakMonoBehaviour.teleport.fromSpawnSent = true;
                OutgoingMessages.SendResurrect(KukouriTime.Get(), KrakMonoBehaviour.world.playerStartPosition);
                OutgoingMessages.recentMapPoints.Add(
                    KrakMonoBehaviour.world.playerStartPosition
                );
            }

            PNode preLatest = PNode.Create(0, 0);
            PNode latest = PNode.Create(0, 0);

            List<PNode> path = new List<PNode>(KrakMonoBehaviour.teleport.teleportingPath); //copy

            int max = KrakMonoBehaviour.teleport.MAX_IN_TICK;
            if (path.Count < max) max = path.Count;

            for (int i = 0; i < max; i++)
            {
                preLatest = latest;
                PNode node = path[i];
                OutgoingMessages.recentMapPoints.Add(new BasicTypes.Vector2i(node.X, node.Y));
                latest = node;

                KrakMonoBehaviour.teleport.teleportingPath.RemoveAt(0);
            }

            if (KrakMonoBehaviour.teleport.teleportingPath.Count < 1)
            {
                /* FINISHED */

                if (KrakMonoBehaviour.teleport.toTake)
                {
                    if (!KrakMonoBehaviour.teleport.takeSent)
                    {
                        KrakMonoBehaviour.teleport.takeSent = true;
                        if (KrakMonoBehaviour.teleport.giftTake)
                        {
                            OutgoingMessages.SendRequestItemFromGiftBoxMessage(new Vector2i(latest.X, latest.Y));
                        }
                        else if (KrakMonoBehaviour.teleport.itemDrop)
                        {
                            OutgoingMessages.SendDropItemMessage(new Vector2i(latest.X, latest.Y), KrakMonoBehaviour.teleport.itemToDrop, 1, KrakMonoBehaviour.playerData.GetInventoryData(KrakMonoBehaviour.teleport.itemToDrop));
                            return; //Drop needs a time
                        }
                        else
                        {
                            Vector2 endVec2 = new Vector2(latest.X, latest.Y);
                            foreach (CollectableData collectable in KrakMonoBehaviour.world.collectables)
                            {
                                Vector2 collectableVec2 = new Vector2(collectable.posX, collectable.posY);
                                float dist = Vector2.Distance(collectableVec2, endVec2);
                                if (dist < 5) OutgoingMessages.SendCollectCollectableMessage(collectable.id);
                            }
                        }
                    }

                    if (KrakMonoBehaviour.teleport.backFromSpawn && !KrakMonoBehaviour.teleport.backFromSpawnSent)
                    {
                        KrakMonoBehaviour.teleport.backFromSpawnSent = true;
                        OutgoingMessages.SendResurrect(KukouriTime.Get(), KrakMonoBehaviour.world.playerStartPosition);
                        OutgoingMessages.recentMapPoints.Add(
                            KrakMonoBehaviour.world.playerStartPosition
                        );
                    }

                    List<PNode> pathBack = new List<PNode>(KrakMonoBehaviour.teleport.teleportingBackPath); //copy

                    int maxBack = KrakMonoBehaviour.teleport.MAX_IN_TICK;
                    if (pathBack.Count < maxBack) maxBack = pathBack.Count;

                    for (int i = 0; i < maxBack; i++)
                    {
                        PNode node = pathBack[i];
                        OutgoingMessages.recentMapPoints.Add(new Vector2i(node.X, node.Y));
                        latest = node;

                        KrakMonoBehaviour.teleport.teleportingBackPath.RemoveAt(0);
                    }

                    if (KrakMonoBehaviour.teleport.teleportingBackPath.Count < 1)
                    {
                        Vector2 mapPoint = utilities.toFloat(new Vector2(latest.X, latest.Y));
                        KrakMonoBehaviour.localPlayer.myTransform.position = new Vector3(mapPoint.x, mapPoint.y, 0);

                        KrakMonoBehaviour.teleport.isTeleporting = false;
                    }
                }
                else
                {
                    Vector2 mapPoint = utilities.toFloat(new Vector2(latest.X, latest.Y));
                    KrakMonoBehaviour.localPlayer.myTransform.position = new Vector3(mapPoint.x, mapPoint.y, 0);

                    KrakMonoBehaviour.teleport.isTeleporting = false;
                }
            }
        }
    }
}

