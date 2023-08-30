using System;
using PaxzClient;
using BasicTypes;

namespace PaxzClient.pathfinding
{
    public class ShiukiAI : TileProvider
    {
        // Some kind of 2D map. How you implement GetWorld() is up to you.
        public bool[,] map;

        public ShiukiAI(int width, int height) : base(width, height)
        {
            map = new bool[width, height];
        }

        public override void ResetSize(int width, int height)
        {
            base.ResetSize(width, height);
            map = new bool[width, height];
        }

        public override bool IsTileWalkable(int x, int y)
        {
            var blockType = krak.KrakMonoBehaviour.world.GetBlockType(new Vector2i(x, y));

            if (!krak.KrakMonoBehaviour.world.IsMapPointInWorld(new Vector2i(x, y))) return false;

            if (this.IsBlockCloud(blockType)) return true;

            if (ConfigData.IsBlockPlatform(blockType) || blockType == World.BlockType.EntrancePortal)
                if (krak.KrakMonoBehaviour.lastpos.y <= y)
                    return true;

            if (ConfigData.IsAnyDoor(blockType))
                if (krak.KrakMonoBehaviour.worldController.DoesPlayerHaveRightToGoDoorForCollider(new Vector2i(x, y)))
                    return true;

            if(ConfigData.IsBlockBattleBarrier(blockType))
            {
                BattleBarrierBasicData barrier = new BattleBarrierBasicData(1);
                barrier.SetViaBSON(
                    krak.KrakMonoBehaviour.world.GetWorldItemData(new Vector2i(x, y)).GetAsBSON()
                );
                if (barrier.isOpen) return true;
            }
            
            if (blockType == World.BlockType.NetherKey || blockType == World.BlockType.JetRaceForcefieldStart ||
                blockType == World.BlockType.JetRaceFinishline || blockType == World.BlockType.JetRaceCrestGold)
                return true;

            if (ConfigData.doesBlockHaveCollider[(int)blockType])
                return false;

            return true;
        }

        public override bool IsBlockInstaKillOn(int x, int y)
        {
            return this.IsBlockInstakill(krak.KrakMonoBehaviour.world.GetBlockType(new Vector2i(x, y)));
        }
    }
}
