using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace krak.Auto
{
    internal class AutoDrop
    {
        public static void DropSeed(int dropAT)
        {
            var inventory = KrakMonoBehaviour.playerData.GetInventoryAsOrderedByInventoryItemType();
            for (int i = 0; i < inventory.Count(); i++)
            {

                PlayerData.InventoryItemType itemType = inventory[i].itemType;
                if (itemType == PlayerData.InventoryItemType.Seed)
                {
                    short count = KrakMonoBehaviour.playerData.GetCount(inventory[i]);
                    if (count > dropAT - 1)
                    {
                        KrakMonoBehaviour.localPlayer.DropItems(inventory[i], count);
                    }
                }
            }
        }

        public static void DropBlock (int dropAT)
        {
            var inventory = KrakMonoBehaviour.playerData.GetInventoryAsOrderedByInventoryItemType();
            for (int i = 0; i < inventory.Count(); i++)
            {

                PlayerData.InventoryItemType itemType = inventory[i].itemType;
                if (itemType == PlayerData.InventoryItemType.Block)
                {
                    short count = KrakMonoBehaviour.playerData.GetCount(inventory[i]);
                    if (count > dropAT - 1)
                    {
                        KrakMonoBehaviour.localPlayer.DropItems(inventory[i], count);
                    }
                }
            }
        }
    }
}
