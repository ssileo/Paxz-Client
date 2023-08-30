using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BasicTypes;
using Il2CppSystem.Threading;
using Kernys.Bson;

namespace krak.auto
{
    class AutoMine
    {

        private static Player player;
        public static bool mining = false;


        public static void Mine()
        {
            
                Vector2i target = GetTargetPos();

                var currentpos = KrakMonoBehaviour.localPlayer.currentPlayerMapPoint;

                if (currentpos.x > target.x)
                {
                    MineLeft(target.x);
                }
                else
                {
                    MineRight(target.x);
                }

                Thread.Sleep(50);
                MineDown(target.y);
                Thread.Sleep(50);
                ExitMine();
           

        }

        public static void Start()
        {
            player = KrakMonoBehaviour.localPlayer;
            mining = true;
            // SceneLoader.CheckIfWeCanGoFromWorldToWorld(KrakMonoBehaviour.world.worldName, "", null, false, null);
            Thread.Sleep(2000);
            var sus = new Il2CppSystem.Threading.Thread((Il2CppSystem.Threading.ThreadStart)auto.AutoMine.Mine);
            sus.Start();
        }

        public static void End()
        {
            Krak.Logger.LogMessage("[AutoMine] -> Ending auto mine!");
            OutgoingMessages.LeaveWorld();
            mining = false;
            player = null;
            Krak.Logger.LogMessage("[AutoMine] -> Ended!");
        }

        public static void ExitMine()
        {
            OutgoingMessages.SendRequestGeneratedMineExitMessage(KrakMonoBehaviour.localPlayer.currentPlayerMapPoint);
            Krak.Logger.LogMessage("[AutoMine] -> Succesfully exited mine!");
        }

        public static void JoinMine()
        {
            // OutgoingMessages.SendTryToJoinMessage("MINEWORLD", true, (World.BasicWorldBiome)0);

            
            SceneLoader.JoinDynamicWorld("MINEWORLD", "0", false);
            OutgoingMessages.SendTryToJoinMessage("MINEWORLD", true, World.BasicWorldBiome.Forest);

            Krak.Logger.LogMessage("[AutoMine] -> Joined mine");
        }

        public static Vector2i GetTargetPos()
        {
            for (int y = 0; y < KrakMonoBehaviour.world.worldSizeY; y++)
            {
                for (int x = 0; x < KrakMonoBehaviour.world.worldSizeY; x++)
                {
                    var vec = new Vector2i(x, y);
                    var block = KrakMonoBehaviour.world.GetBlockType(vec);

                    if (block == World.BlockType.PortalMineExit)
                    {
                        Krak.Logger.LogMessage("[AutoMine] -> Exit Portal found: " + vec);
                        return vec;
                    }
                }
            }
            return new Vector2i(-1, -1);

            // OutgoingMessages.SendRequestGeneratedMineExitMessage();
            // OutgoingMessages.SendGoFromPortalMessage();

        }

        #region Mining Directions
        private static void MineLeft(int targetx)
        {
            
            bool targetPos = false;
            while (!targetPos)
            {
                if (mining)
                {
                    Vector2i leftpos = KrakMonoBehaviour.localPlayer.currentPlayerLeftMapPoint;
                    Vector2i currentpos = KrakMonoBehaviour.localPlayer.currentPlayerMapPoint;

                    if (currentpos.x == targetx)
                    {
                        Krak.Logger.LogMessage("[AutoMine] -> Target X position reached!");
                        targetPos = true;
                    }
                    else
                    {
                        int hitsleft;
                        do
                        {
                            hitsleft = KrakMonoBehaviour.world.GetBlockHitsLeft(leftpos.x, leftpos.y) / 200;
                            OutgoingMessages.SendHitBlockMessage(leftpos, Il2CppSystem.DateTime.UtcNow, false);
                            Thread.Sleep(110);
                        } while (hitsleft > 0);
                        
                        
                        var newTrans = utilities.ConvertMapPointToWorldPoint(KrakMonoBehaviour.localPlayer.currentPlayerLeftMapPoint);
                        KrakMonoBehaviour.localPlayer.myTransform.position = new Vector3(newTrans.x, KrakMonoBehaviour.localPlayer.myTransform.position.y, KrakMonoBehaviour.localPlayer.myTransform.position.z);
                        Thread.Sleep(50);
                    }
                }
                
            }
        }


        private static void MineRight(int targetx)
        {

            bool targetPos = false;
            while (!targetPos)
            {
                if (mining)
                {
                    Vector2i rightpos = KrakMonoBehaviour.localPlayer.currentPlayerRightMapPoint;
                    Vector2i currentpos = KrakMonoBehaviour.localPlayer.currentPlayerMapPoint;

                    if (currentpos.x == targetx)
                    {
                        Krak.Logger.LogMessage("[AutoMine] -> Target X position reached!");
                        targetPos = true;
                    }
                    else
                    {
                        int hitsleft;
                        do
                        {
                            hitsleft = KrakMonoBehaviour.world.GetBlockHitsLeft(rightpos.x, rightpos.y) / 200;
                            OutgoingMessages.SendHitBlockMessage(rightpos, Il2CppSystem.DateTime.UtcNow, false);
                            Thread.Sleep(110);
                        } while (hitsleft > 0);

                        
                            var newTrans = utilities.ConvertMapPointToWorldPoint(KrakMonoBehaviour.localPlayer.currentPlayerRightMapPoint);
                            KrakMonoBehaviour.localPlayer.myTransform.position = new Vector3(newTrans.x, KrakMonoBehaviour.localPlayer.myTransform.position.y, KrakMonoBehaviour.localPlayer.myTransform.position.z);
                        Thread.Sleep(55);

                    }
                }

            }
        }

        private static void MineDown(int targety)
        {

            bool targetPos = false;
            while (!targetPos)
            {
                if (mining)
                {
                    Vector2i belowpos = KrakMonoBehaviour.localPlayer.currentPlayerBelowMapPoint;
                    Vector2i currentpos = KrakMonoBehaviour.localPlayer.currentPlayerMapPoint;

                    if (currentpos.y == targety)
                    {
                        Krak.Logger.LogMessage("[AutoMine] -> Target X position reached!");
                        targetPos = true;
                    }
                    else
                    {
                        var hitsleft = KrakMonoBehaviour.world.GetBlockHitsLeft(belowpos.x, belowpos.y) / 200;
                        if (hitsleft > 0)
                        {
                            OutgoingMessages.SendHitBlockMessage(belowpos, Il2CppSystem.DateTime.UtcNow, false);
                            Thread.Sleep(110);
                        }
                        else
                        {
                            Thread.Sleep(30);
                            
                        }

                    }
                }

            }
        }

        #endregion


        public static void validMineBlock(Vector2i pos)
        {
            
        }



    }
}
