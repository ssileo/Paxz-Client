using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace krak
{
    internal class GemCalculator
    {
        private static int totalgems;


        public static int GetGemAmount(World.BlockType blockType)
        {
            if (ConfigData.IsFish(blockType))
            {
                return ConfigData.GetFishRecycleValueForFishRecycler(blockType);
            }
            if (ConfigData.IsBlockMiningGemstone(blockType))
            {
                return ConfigData.GetGemstoneRecycleValueForMiningGemstoneRecycler(blockType);
            }
            if (ConfigData.IsConsumableTreasurePouch(blockType))
            {
                return ConfigData.GetTreasurePouchRewardAmount(blockType);
            }

            return 0;
        }

        public static int DisplayGemAmmount()
        {
            if (KrakMonoBehaviour.world != null)
            {
                totalgems = 0;

                foreach (CollectableData collectable in KrakMonoBehaviour.world.collectables)
                {
                    totalgems += (GetGemAmount(collectable.blockType) * collectable.amount);
                }
            }
            else
            {
                totalgems = 0;
            }
            return totalgems;
        }

        public static int[] GemWorldAmount()
        {
            Il2CppSystem.Collections.Generic.List<CollectableData> WorldCollectables = KrakMonoBehaviour.world.collectables;
            int WorldGems = 0;
            int mgems = 0;
            int fgems = 0;
            int gemBags = 0;

            foreach(CollectableData c in WorldCollectables)
            {
                World.BlockType blockType = c.blockType;
                
                if (ConfigData.IsFish(blockType))
                {
                    int x = ConfigData.GetFishRecycleValueForFishRecycler(blockType);
                    int y = x * c.amount;

                    WorldGems += y;
                    fgems += y;

                }
                else if (ConfigData.IsBlockMiningGemstone(blockType))
                {
                    int x =ConfigData.GetGemstoneRecycleValueForMiningGemstoneRecycler(blockType);
                    int y = x * c.amount;

                    WorldGems += y;
                    mgems += y;
                }
                else if (ConfigData.IsConsumableTreasurePouch(blockType))
                {
                    int x =ConfigData.GetTreasurePouchRewardAmount(blockType);
                    int y = x * c.amount;

                    WorldGems += y;
                    gemBags += y;
                }
            }
            // hi
            int[] f = { WorldGems, mgems, fgems, gemBags };
            return f;
        }
    }
}
