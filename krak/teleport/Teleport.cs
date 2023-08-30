using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTypes;
using PaxzClient;
using krak;
using PaxzClient.pathfinding;


namespace PaxzClient.teleport
{
    public class Teleport
    {

        public bool isTeleporting = false;
        public bool fromSpawn = false;
        public bool fromSpawnSent = false;
        public bool backFromSpawn = false;
        public bool backFromSpawnSent = false;
        public int sentTicks = 0;

        #region toTakeActions
        public bool toTake = false;
        public bool giftTake = false;
        public bool takeSent = false;
        public bool itemDrop = false;
        public PlayerData.InventoryKey itemToDrop = PlayerData.InventoryKey.GetNoneBlockKey();
        #endregion

        public GiftBoxData giftData = new GiftBoxData(1);

        public List<PNode> teleportingPath;
        public List<PNode> teleportingBackPath;

        private Pathfinding pather;
        private TileProvider provider;

        public readonly int MAX_IN_TICK = 15;

        public Teleport()
        {
            this.pather = new Pathfinding();
            this.provider = new ShiukiAI(1, 1);
            
        }

        private void ResetMap()
        {
            this.provider.ResetSize(krak.KrakMonoBehaviour.world.worldSizeX, krak.KrakMonoBehaviour.world.worldSizeY);
        }

        public PathfindingResult tryTeleportTo(int fromX, int fromY, int toX, int toY, bool log = true)
        {

            if(this.isTeleporting)
            {
                if (log) krak.Methods.SendMessage("Already teleporting...", false);
                return PathfindingResult.CANCELLED;
            }

            this.ResetMap();
            this.toTake = false;

            Vector2i worldSpawn = new Vector2i(krak.KrakMonoBehaviour.world.playerStartPosition.x, krak.KrakMonoBehaviour.world.playerStartPosition.y);

            List<PNode> pathTo;
            PathfindingResult pathToResult = this.pather.Run(fromX, fromY, toX, toY, this.provider, out pathTo);

            List<PNode> pathToSpawn;
            PathfindingResult pathToSpawnResult = this.pather.Run(worldSpawn.x, worldSpawn.y, toX, toY, this.provider, out pathToSpawn);

            bool fromSpawn = false;

            if (pathToSpawnResult == PathfindingResult.SUCCESSFUL && (pathToResult != PathfindingResult.SUCCESSFUL || pathToSpawn.Count < pathTo.Count))
            {
                pathTo = pathToSpawn;
                pathToResult = pathToSpawnResult;
                fromSpawn = true;
            }

            if (pathToResult != PathfindingResult.SUCCESSFUL)
            {
                if (log) krak.Methods.SendMessage($"PathTo not found: {pathToResult.ToString()}", false);
                return pathToResult;
            }

            pathTo.RemoveAt(0);

            if (log) krak.Methods.SendMessage($"Teleporting {pathTo.Count} Tiles... (~{pathTo.Count / this.MAX_IN_TICK * 1.5f}s.)", false);

            this.fromSpawn = fromSpawn;
            this.fromSpawnSent = false;
            this.teleportingPath = pathTo;
            this.isTeleporting = true;
            this.sentTicks = 0;

            krak.Utils.Timers.onTeleportTimer(null, null);

            return pathToResult;
        }

        public void tryTeleportToTake(int fromX, int fromY, int toX, int toY, bool log = true, bool toDrop = false)
        {
            if (this.isTeleporting)
            {
                if (log) krak.Methods.SendMessage("Already teleporting...", false);
                return;
            }

            this.ResetMap();
            this.itemDrop = false;
            this.toTake = false;
            this.giftTake = false;
            this.backFromSpawn = false;

            Vector2i worldSpawn = new Vector2i(krak.KrakMonoBehaviour.world.playerStartPosition.x, krak.KrakMonoBehaviour.world.playerStartPosition.y);

            List<PNode> pathTo;
            PathfindingResult pathToResult = this.pather.Run(fromX, fromY, toX, toY, this.provider, out pathTo);

            List<PNode> pathToSpawn;
            PathfindingResult pathToSpawnResult = this.pather.Run(worldSpawn.x, worldSpawn.y, toX, toY, this.provider, out pathToSpawn);

            bool fromSpawn = false;

            if (pathToSpawnResult == PathfindingResult.SUCCESSFUL && (pathToResult != PathfindingResult.SUCCESSFUL || pathToSpawn.Count < pathTo.Count))
            {
                pathTo = pathToSpawn;
                pathToResult = pathToSpawnResult;
                fromSpawn = true;
            }

            WorldItemBase worldItemBase = krak.KrakMonoBehaviour.world.GetWorldItemData(new Vector2i(toX, toY));
            if(worldItemBase != null)
            {
                switch (worldItemBase.blockType)
                {
                    case World.BlockType.GiftBox:
                    case World.BlockType.LabGiftBox:
                    case World.BlockType.NetherGiftBox:

                        this.giftData.SetViaBSON(worldItemBase.GetAsBSON());

                        if (this.giftData.takeAmount < 1 || this.giftData.itemAmount < 1)
                        {
                            if(log) krak.Methods.SendMessage("GiftBox is empty!", false);
                            return;
                        }

                        this.giftTake = true;
                        this.toTake = true;

                        break;
                }
            }

            if(!toDrop && !this.giftTake)
            {
                Vector2i endPoint = new Vector2i(toX, toY);

                foreach (CollectableData collectable in krak.KrakMonoBehaviour.world.collectables)
                {
                    if (collectable.mapPoint.Equals(endPoint))
                    {
                        this.toTake = true;
                        break;
                    }
                }
            }

            if(!toDrop && !this.toTake)
            {
                /* PortalTeleportAction */
                switch (krak.KrakMonoBehaviour.world.GetBlockType(new Vector2i(toX, toY)))
                {
                    case World.BlockType.Portal:
                    case World.BlockType.PortalPassword:
                    case World.BlockType.PortalFactionDark:
                    case World.BlockType.PortalFactionLight:
                    case World.BlockType.PortalCryptic:
                    case World.BlockType.VortexPortal:

                        PortalData portal = new PortalData(1);
                        portal.SetViaBSON(worldItemBase.GetAsBSON());

                        if (portal.entryPointID == "")
                        {
                            krak.Methods.SendMessage("PortalId is null.", false);
                            return;
                        }

                        SceneLoader.CheckIfWeCanGoFromWorldToWorld(krak.KrakMonoBehaviour.world.worldName, portal.entryPointID, null, false, null);
                        string sus = $"Trying join {krak.KrakMonoBehaviour.world.worldName} {portal.entryPointID}";
                        krak.Krak.Logger.LogMessage(sus);

                        return;
                }

                if (log) krak.Methods.SendMessage("Nothing found...", false);
                return;
            }

            try
            {
                if (pathToResult != PathfindingResult.SUCCESSFUL)
                {
                    if (log) krak.Methods.SendMessage($"PathTo not found: {pathToResult.ToString()}", false);
                    return;
                }

                List<PNode> pathToBack;
                PNode endNode = PNode.Create(0, 0);
                for (int i = 0; i < pathTo.Count; i++) { endNode = pathTo[i]; }

                PathfindingResult pathToBackResult = this.pather.Run(endNode.X, endNode.Y, fromX, fromY, this.provider, out pathToBack);

                List<PNode> pathToBackSpawn;
                PathfindingResult pathToBackSpawnResult = this.pather.Run(worldSpawn.x, worldSpawn.y, fromX, fromY, this.provider, out pathToBackSpawn);

                if (pathToBackSpawnResult == PathfindingResult.SUCCESSFUL && (pathToBackResult != PathfindingResult.SUCCESSFUL || pathToBackSpawn.Count < pathToBack.Count))
                {
                    pathToBack = pathToBackSpawn;
                    pathToBackResult = pathToBackSpawnResult;
                    this.backFromSpawn = true;
                }

                if (pathToBackResult != PathfindingResult.SUCCESSFUL)
                {
                    if (log) krak.Methods.SendMessage($"PathToBack not found: {pathToBack.ToString()}", false);
                    return;
                }

                pathTo.RemoveAt(0);

                if (log) krak.Methods.SendMessage($"Trying to " + (toDrop ? "drop" : "collect") + $". {pathTo.Count + pathToBack.Count} Tiles... (~{ (pathTo.Count + pathToBack.Count) / this.MAX_IN_TICK * 1.5f}s.)", false);

                this.fromSpawn = fromSpawn;
                this.fromSpawnSent = false;
                this.teleportingPath = pathTo;
                this.teleportingBackPath = pathToBack;
                this.takeSent = false;
                this.sentTicks = 0;

                if(toDrop)
                {
                    this.itemDrop = true;
                    //this.itemToDrop = Globals.itemToolTipKey;
                    this.toTake = true;
                    this.giftTake = false;
                }
                this.backFromSpawnSent = false;
                this.isTeleporting = true;

                krak.Utils.Timers.onTeleportTimer(null, null);
            }catch(Exception e)
            {
                krak.Methods.SendMessage("PathToBack not found.", false);
            }
        }

        public PathfindingResult tryTeleportTo(Vector2i from, Vector2i to, bool log = true){ return this.tryTeleportTo(from.x, from.y, to.x, to.y, log); }

        public PathfindingResult tryTeleportTo(Vector2i from, Vector2 to, bool log = true) { return this.tryTeleportTo(from.x, from.y, (int)to.x, (int)to.y, log); }

        public void tryTeleportToTake(Vector2i from, Vector2i to, bool log = true, bool toDrop = false) { this.tryTeleportToTake(from.x, from.y, to.x, to.y, log, toDrop); }

        public void tryTeleportToTake(Vector2i from, Vector2 to, bool log = true, bool toDrop = false) { this.tryTeleportToTake(from.x, from.y, (int)to.x, (int)to.y, log, toDrop); }

    }
}
