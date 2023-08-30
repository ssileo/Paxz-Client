using System;
using System.Text;
using System.Threading.Tasks;
using krak;
using UnityEngine;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using System.Linq;
using BasicTypes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.IO;
using System.Threading;
using krak.Api;

namespace krak
{
    internal class Methods
    {
        public static void GetWorldData()
        {

            Il2CppSystem.DateTime dt = KrakMonoBehaviour.world.GetLockWorldHelper().GetLastActivatedTime();
            string owner = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayerWhoOwnsLockName();
            List<string> accessed = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayersWhoHaveAccessToLock();
            List<string> minoraccessed = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayersWhoHaveMinorAccessToLock();
            bool isBattleOn = KrakMonoBehaviour.world.GetLockWorldHelper().GetIsBattleOn();
            bool isPunchOn = KrakMonoBehaviour.world.GetLockWorldHelper().GetIsPunchingAllowed();
            bool isOpen = KrakMonoBehaviour.world.GetLockWorldHelper().GetIsOpen();


            SendMessage("<#FF00FF>[krak]:<#FFFFFF> World lock data", false);
            string idk = $"<#FFFFFF>Last active: {dt}";
            SendMessage(idk, false);
            SendMessage("<#FFFFFF>Current DateTime: " + Il2CppSystem.DateTime.UtcNow, false);
            SendMessage("<#FFFFFF>Owned by: " + owner, false);
            for (int i = 0; i < accessed.Count; i++)
            {
                string[] access = accessed[i].Split(';');
                SendMessage("<#FFFFFF>Full rights : " + access[1], false);
            }
            for (int i = 0; i < minoraccessed.Count; i++)
            {
                string[] access = minoraccessed[i].Split(';');
                SendMessage("minor rights: " + access[1], false);
            }
            SendMessage("Punch = " + isPunchOn, false);
            SendMessage("World open = " + isOpen, false);
            SendMessage("PVP = " + isBattleOn, false);
            int gems = GemCalculator.DisplayGemAmmount();
            SendMessage("Gems = " + gems, false);

        }

        public static void SellGems()
        {
            int gems = 0;

            var inventory = KrakMonoBehaviour.playerData.GetInventoryAsOrderedByInventoryItemType();

            for (int i = 0; i < inventory.Count(); i++)
            {
                short count = KrakMonoBehaviour.playerData.GetCount(inventory[i]);
                World.BlockType blockType = inventory[i].blockType;

                // mgems
                if (ConfigData.IsBlockMiningGemstone(blockType))
                {
                    OutgoingMessages.RecycleMiningGemstone(inventory[i], count);
                    gems += GemCalculator.GetGemAmount(blockType) * count;
                    KrakMonoBehaviour.playerData.RemoveItemsFromInventory(inventory[i], count);
                }

                // fgems
                if (ConfigData.IsFish(blockType))
                {
                    OutgoingMessages.RecycleFish(inventory[i], count);
                    gems += GemCalculator.GetGemAmount(blockType) * count;
                    KrakMonoBehaviour.playerData.RemoveItemsFromInventory(inventory[i], count);
                }
            }

            ChatUI.SendMinigameMessage("Recycled: " + gems + " gems.");
            KrakMonoBehaviour.playerData.AddGems(gems);
        }

        public static bool Warp(string str)
        {
            string[] bob = str.Split(' ');
            if (bob[0] == "/warp" || bob[0] == "/w")
            {
                if (bob.Length > 2)
                {
                    SceneLoader.CheckIfWeCanGoFromWorldToWorld(bob[1], bob[2], null, false, null);
                    return false;
                }
                else
                {
                    SceneLoader.CheckIfWeCanGoFromWorldToWorld(bob[1], "", null, false, null);
                    return false;
                }

            }
            return true;
        }

        public static bool Drop(string str)
        {
            string[] strr = str.Split(' ');
            if (str == "/drop all")
            {
                var inventory = KrakMonoBehaviour.playerData.GetInventoryAsOrderedByInventoryItemType();
                for (int i = 0; i < inventory.Count; i++)
                {
                    short count = KrakMonoBehaviour.playerData.GetCount(inventory[i]);
                    KrakMonoBehaviour.localPlayer.DropItems(inventory[i], count);
                    ChatUI.SendLogMessage($"[krak] Dropped: {count} {inventory[i]}");
                }
                return false;
            }
            else if (strr[0] == "/drop")
            {
                try
                {
                    int i = System.Convert.ToInt32(strr[1]);
                    var inventory = KrakMonoBehaviour.playerData.GetInventoryAsOrderedByInventoryItemType();
                    short count = KrakMonoBehaviour.playerData.GetCount(inventory[i]);
                    KrakMonoBehaviour.localPlayer.DropItems(inventory[i], count);
                    ChatUI.SendLogMessage($"[krak] Dropped: {count} {inventory[i]}");
                    return false;
                }
                catch
                {
                    Krak.Logger.LogMessage("Index out of range || invalid input");
                    return false;
                }
            }
            return true;
        }

        public static void Fastdrop()
        {
            var inventory = KrakMonoBehaviour.playerData.GetInventoryAsOrderedByInventoryItemType();
            short count = KrakMonoBehaviour.playerData.GetCount(inventory[0]);
            KrakMonoBehaviour.localPlayer.DropItems(inventory[0], count);
        }

        public static bool BytesOrGems(string str)
        {
            string[] strr = str.Split(' ');
            if (strr[0] == "/gems")
            {
                KrakMonoBehaviour.playerData.AddGems(System.Convert.ToInt32(strr[1]));
                return false;
            }
            if (strr[0] == "/bytes")
            {
                KrakMonoBehaviour.playerData.AddByteCoins(System.Convert.ToInt32(strr[1]));
                return false;
            }
            if (strr[0] == "/xp")
            {
                KrakMonoBehaviour.playerData.GiveExperience(System.Convert.ToInt32(strr[1]));
                return false;
            }
            return true;
        }

        public static void Penis()
        {
            Vector2i localplayer = KrakMonoBehaviour.localPlayer.currentPlayerMapPoint;
            Vector2i wwnh = localplayer;
            int row = +2;
            Il2CppSystem.DateTime dateTime = KrakMonoBehaviour.localPlayer.raceStartTime;

            for (int i = 0; i < 5; i++)
            {
                // row 1 colom 1
                wwnh.x = localplayer.x - 2;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 1 colom 2
                wwnh.x = localplayer.x - 1;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 1 colom 3
                wwnh.x = localplayer.x;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 1 colom 4
                wwnh.x = localplayer.x + 1;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 1 colom 5
                wwnh.x = localplayer.x + 2;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);

                row = 1;
                // row 2 colom 1
                wwnh.x = localplayer.x - 2;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 2 colom 2
                wwnh.x = localplayer.x - 1;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 2 colom 3
                wwnh.x = localplayer.x;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 2 colom 4
                wwnh.x = localplayer.x + 1;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 2 colom 5
                wwnh.x = localplayer.x + 2;
                wwnh.y = localplayer.y + row;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);

                // row 3 colom 1
                wwnh.x = localplayer.x - 2;
                wwnh.y = localplayer.y;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 3 colom 2
                wwnh.x = localplayer.x - 1;
                wwnh.y = localplayer.y;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 3 colom 3
                wwnh.x = localplayer.x;
                wwnh.y = localplayer.y;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 3 colom 4
                wwnh.x = localplayer.x + 1;
                wwnh.y = localplayer.y;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                // row 3 colom 5
                wwnh.x = localplayer.x + 2;
                wwnh.y = localplayer.y;
                OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);

                if (localplayer.y > 4)
                {
                    // row 4 colom 1
                    wwnh.x = localplayer.x - 2;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 4 colom 2
                    wwnh.x = localplayer.x - 1;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 4 colom 3
                    wwnh.x = localplayer.x;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 4 colom 4
                    wwnh.x = localplayer.x + 1;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 4 colom 5
                    wwnh.x = localplayer.x + 2;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                }


                if (localplayer.y > 5)
                {
                    row = 2;
                    // row 5 colom 1
                    wwnh.x = localplayer.x - 2;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 5 colom 2
                    wwnh.x = localplayer.x - 1;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 5 colom 3
                    wwnh.x = localplayer.x;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 5 colom 4
                    wwnh.x = localplayer.x + 1;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                    // row 5 colom 5
                    wwnh.x = localplayer.x + 2;
                    wwnh.y = localplayer.y - row;
                    OutgoingMessages.SendHitBlockMessage(wwnh, dateTime, false);
                    OutgoingMessages.SendHitBlockBackgroundMessage(wwnh, dateTime);
                }

            }
            ChatUI.SendLogMessage("[krak] Nuked!");
        }
        public static void BlockAll(int block)
        {
            Vector2i localplayer = KrakMonoBehaviour.localPlayer.currentPlayerMapPoint;
            Vector2i wwnh = localplayer;
            int row = 2;
            // row 1 colom 1
            wwnh.x = localplayer.x - 2;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 1 colom 2
            wwnh.x = localplayer.x - 1;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 1 colom 3
            wwnh.x = localplayer.x;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 1 colom 4
            wwnh.x = localplayer.x + 1;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 1 colom 5
            wwnh.x = localplayer.x + 2;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);

            row = 1;
            // row 2 colom 1
            wwnh.x = localplayer.x - 2;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 2 colom 2
            wwnh.x = localplayer.x - 1;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 2 colom 3
            wwnh.x = localplayer.x;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 2 colom 4
            wwnh.x = localplayer.x + 1;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 2 colom 5
            wwnh.x = localplayer.x + 2;
            wwnh.y = localplayer.y + row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);

            // row 3 colom 1
            wwnh.x = localplayer.x - 2;
            wwnh.y = localplayer.y;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 3 colom 2
            wwnh.x = localplayer.x - 1;
            wwnh.y = localplayer.y;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);

            // local player;

            // row 3 colom 4
            wwnh.x = localplayer.x + 1;
            wwnh.y = localplayer.y;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 3 colom 5
            wwnh.x = localplayer.x + 2;
            wwnh.y = localplayer.y;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);

            // row 4 colom 1
            wwnh.x = localplayer.x - 2;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 4 colom 2
            wwnh.x = localplayer.x - 1;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 4 colom 3
            wwnh.x = localplayer.x;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 4 colom 4
            wwnh.x = localplayer.x + 1;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 4 colom 5
            wwnh.x = localplayer.x + 2;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);

            row = 2;
            // row 5 colom 1
            wwnh.x = localplayer.x - 2;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 5 colom 2
            wwnh.x = localplayer.x - 1;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 5 colom 3
            wwnh.x = localplayer.x;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 5 colom 4
            wwnh.x = localplayer.x + 1;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);
            // row 5 colom 5
            wwnh.x = localplayer.x + 2;
            wwnh.y = localplayer.y - row;
            OutgoingMessages.SendSetBlockMessage(wwnh, (World.BlockType)block);

        }

        public static void CustomDeath(int index)
        {
            List<World.BlockType> deathType = new List<World.BlockType>() { };

            //deathType.Add(World.BlockType.BasicEyeballs);
            //deathType.Add(World.BlockType.ZombieTrapRed);
            //deathType.Add(World.BlockType.ZombieTrap);
            //deathType.Add(World.BlockType.Acid);
            //deathType.Add(World.BlockType.Lava);
            deathType.Add(World.BlockType.SpikeBall);
            deathType.Add(World.BlockType.FrostTrap);
            deathType.Add(World.BlockType.FireTrap);
            deathType.Add(World.BlockType.ElectricTrap);
            deathType.Add(World.BlockType.ElectricChair);




            KrakMonoBehaviour.localPlayer.HitPlayerFromBlock(99999, deathType[index], KrakMonoBehaviour.localPlayer.currentPlayerMapPoint);
        }

        public static void SendMessage(string message, ChatMessage.ChannelTypes channel, bool doPre)
        {
            string prefix = "";
            if (doPre)
            {
                prefix = "<#FF00FF><b>krak<#FFFFFF>";
            }

            ChatMessage msg = new ChatMessage($"{message}", Il2CppSystem.DateTime.Now, channel, "", prefix, "");
            KrakMonoBehaviour.chatUI.NewMessage(msg);
        }
        public static void SendMessage(string message, bool doPre)
        {
            string prefix = "";
            if (doPre)
            {
                prefix = "<#FF00FF><b>krak<#FFFFFF>";
            }
            ChatMessage msg = new ChatMessage($"{message}", Il2CppSystem.DateTime.Now, ChatMessage.ChannelTypes.SERVER_MESSAGE, "", prefix, "");
            KrakMonoBehaviour.chatUI.NewMessage(msg);
        }

        public static void Scan()
        {

            string color = "<#FFFF66>";
            string color2 = "<#33FF33>";
            string color3 = "<#33FF33>";
            string red = "<#FFFFFF>"; // i know its white stfuuu


            List<World.BlockType> displayedother = new List<World.BlockType> { };
            List<World.BlockType> displayed = new List<World.BlockType> { };

            int num = 0;
            bool isinlist;

            SendMessage($"<#FF00FF>[krak]:<#FF0000><b> Scanning world {color}{KrakMonoBehaviour.world.worldName}<#FF0000>...<#FFFFFF>", false);
            foreach (CollectableData collectable in KrakMonoBehaviour.world.collectables)
            {
                isinlist = false;
                int amountBlock = 0;
                int amountBlockBackground = 0;
                int amountSeed = 0;
                int amountBlockWater = 0;
                int amountWearableItem = 0;
                int amountWeapon = 0;
                int amountThrowable = 0;
                int amountConsumable = 0;
                int amountShard = 0;
                int amountBlueprint = 0;
                int amountFamiliar = 0;
                int amountFAMFood = 0;
                int amountBlockWiring = 0;
                int amountGem = 0;

                if (collectable == null) continue;
                foreach (CollectableData collectable2 in KrakMonoBehaviour.world.collectables)
                {
                    if (collectable2 == null) continue;
                    if (collectable2.blockType != collectable.blockType) continue;


                    if (!collectable2.isGem)
                    {
                        switch (collectable2.inventoryItemType)
                        {
                            case (PlayerData.InventoryItemType.Block): // 0
                                amountBlock += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.BlockBackground): // 1
                                amountBlockBackground += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Seed): // 2
                                amountSeed += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.BlockWater): // 3
                                amountBlockWater += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.WearableItem): // 4
                                amountWearableItem += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Weapon): // 5
                                amountWeapon += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Throwable): // 6
                                amountThrowable += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Consumable): // 7
                                amountConsumable += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Shard): // 8
                                amountShard += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Blueprint): // 9
                                amountBlueprint += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Familiar): // 10
                                amountFamiliar += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.FAMFood): // 11
                                amountFAMFood += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.BlockWiring): // 12
                                amountBlockWiring += collectable2.amount;
                                break;
                        }
                    }
                    else
                    {
                        amountGem += ConfigData.GetGemValue(collectable2.gemType);

                    }

                }
                if (collectable.inventoryItemType == PlayerData.InventoryItemType.Block)
                {
                    for (int i = 0; i < displayed.Count; i++)
                    {
                        if (collectable.blockType == displayed[i])
                        {
                            isinlist = true;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < displayedother.Count; i++)
                    {
                        if (collectable.blockType == displayedother[i])
                        {
                            isinlist = true;
                        }

                    }
                }

                if (!isinlist)
                {
                    if (collectable.inventoryItemType == PlayerData.InventoryItemType.Block)
                        displayed.Add(collectable.blockType);
                    else displayedother.Add(collectable.blockType);
                    if (!collectable.isGem)
                    {
                        switch (collectable.inventoryItemType)
                        {
                            case (PlayerData.InventoryItemType.Block): // 0
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountBlock}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.BlockBackground): // 1
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountBlockBackground}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Seed): // 2
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountSeed}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.BlockWater): // 3
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountBlockWater}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.WearableItem): // 4
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountWearableItem}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Weapon): // 5
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountWeapon}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Throwable): // 6
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountThrowable}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Consumable): // 7
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountConsumable}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Shard): // 8
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountShard}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Blueprint): // 9
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountBlueprint}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.Familiar): // 10
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountFamiliar}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.FAMFood): // 11
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountFAMFood}{color}!{red}", false);
                                break;
                            case (PlayerData.InventoryItemType.BlockWiring): // 12
                                SendMessage($"{color}    {num + 1}.{color3} {collectable.blockType} {color}{collectable.inventoryItemType}{color}; amount = {color2}{amountBlockWiring}{color}!{red}", false);
                                break;
                        }
                        num++;
                    }
                    else
                    {

                        SendMessage($"{color}    {num + 1}.{color3} Gems{color} = {color2}{amountGem}{color}!", false);
                        num++;
                    }


                }
            }
            SendMessage($"<#FF00FF>[krak]:<#FF0000> Found {color3}{KrakMonoBehaviour.world.collectables.Count}<#FF0000> different collectables...<#FFFFFF>{red}", false);
        }

        public static string DumpIDs()
        {
            List<World.BlockType> items = new List<World.BlockType>();
            
            for (int i = 0; i < (int)World.BlockType.END_OF_THE_ENUM; i++)
            {
                items.Add((World.BlockType)i);
            }

            System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();

            foreach (World.BlockType itemType in items)
            {
                lines.Add($"{itemType} - ID: {((int)itemType)}");
            }
            lines.Add("Dumped by krak#7305 -> by discord bot");
            string ok = "Latest item ids dumped by krak#7305\n" + string.Join("\n", lines.ToArray());
            return ok;
        }


        public static double Evaluate(string expression)
        {
            var xsltExpression =
                string.Format("number({0})",
                    new Regex(@"([\+\-\*])").Replace(expression, " ${1} ")
                                            .Replace("/", " div ")
                                            .Replace("%", " mod "));

            return (double)new XPathDocument
                (new StringReader("<r/>"))
                    .CreateNavigator()
                    .Evaluate(xsltExpression);
        }




        public static string LockWorldData4B()
        {
            string data;
            try
            {
                Il2CppSystem.DateTime dt = KrakMonoBehaviour.world.GetLockWorldHelper().GetLastActivatedTime();
                Il2CppSystem.DateTime ct = Il2CppSystem.DateTime.UtcNow;
                string owner = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayerWhoOwnsLockName();
                List<string> accessed = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayersWhoHaveAccessToLock();
                List<string> minoraccessed = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayersWhoHaveMinorAccessToLock();
                int gems = GemCalculator.DisplayGemAmmount();

                dt = dt.AddYears(1);

                string date = $"{dt.Day} {month(dt.Month)} {dt.Year}";
                string cdate = $"{ct.Day} {month(ct.Month)} {ct.Year}";

                System.Collections.Generic.List<string> scannerData = Scannerrr();
                string scannerdata = string.Join("", scannerData.ToArray());
                data = $"{owner} ! {date} ! {cdate} ! {gems} ! {KrakMonoBehaviour.world.worldName} % {scannerdata}";
            }
            catch
            {
                System.Collections.Generic.List<string> scannerData = Scannerrr();
                string scannerdata = string.Join("", scannerData.ToArray());
                int gems = GemCalculator.DisplayGemAmmount();
                data = $"No owner ! No world lock  ! no World lock ! {gems} ! {KrakMonoBehaviour.world.worldName} % {scannerdata}";
            }

            
            return data;
        }
        public static string month(int month)
        {
            string monthS = "";

            switch (month)
            {
                case 1:
                    monthS = "January";
                    break;
                case 2:
                    monthS = "February";
                    break;
                case 3:
                    monthS = "March";
                    break;
                case 4:
                    monthS = "April";
                    break;
                case 5:
                    monthS = "May";
                    break;
                case 6:
                    monthS = "June";
                    break;
                case 7:
                    monthS = "July";
                    break;
                case 8:
                    monthS = "August";
                    break;
                case 9:
                    monthS = "September";
                    break;
                case 10:
                    monthS = "October";
                    break;
                case 11:
                    monthS = "November";
                    break;
                case 12:
                    monthS = "December";
                    break;
            }


            return monthS;
        }
        public static System.Collections.Generic.List<string> Scannerrr()
        {

            System.Collections.Generic.List<string> data = new System.Collections.Generic.List<string>() { };

            List<World.BlockType> displayedother = new List<World.BlockType> { };
            List<World.BlockType> displayed = new List<World.BlockType> { };

            int num = 0;
            bool isinlist;

            foreach (CollectableData collectable in KrakMonoBehaviour.world.collectables)
            {
                isinlist = false;
                int amountBlock = 0;
                int amountBlockBackground = 0;
                int amountSeed = 0;
                int amountBlockWater = 0;
                int amountWearableItem = 0;
                int amountWeapon = 0;
                int amountThrowable = 0;
                int amountConsumable = 0;
                int amountShard = 0;
                int amountBlueprint = 0;
                int amountFamiliar = 0;
                int amountFAMFood = 0;
                int amountBlockWiring = 0;
                int amountGem = 0;

                if (collectable == null) continue;
                foreach (CollectableData collectable2 in KrakMonoBehaviour.world.collectables)
                {
                    if (collectable2 == null) continue;
                    if (collectable2.blockType != collectable.blockType) continue;


                    if (!collectable2.isGem)
                    {
                        switch (collectable2.inventoryItemType)
                        {
                            case (PlayerData.InventoryItemType.Block): // 0
                                amountBlock += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.BlockBackground): // 1
                                amountBlockBackground += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Seed): // 2
                                amountSeed += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.BlockWater): // 3
                                amountBlockWater += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.WearableItem): // 4
                                amountWearableItem += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Weapon): // 5
                                amountWeapon += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Throwable): // 6
                                amountThrowable += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Consumable): // 7
                                amountConsumable += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Shard): // 8
                                amountShard += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Blueprint): // 9
                                amountBlueprint += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.Familiar): // 10
                                amountFamiliar += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.FAMFood): // 11
                                amountFAMFood += collectable2.amount;
                                break;
                            case (PlayerData.InventoryItemType.BlockWiring): // 12
                                amountBlockWiring += collectable2.amount;
                                break;
                        }
                    }
                    else
                    {
                        amountGem += ConfigData.GetGemValue(collectable2.gemType);

                    }

                }
                if (collectable.inventoryItemType == PlayerData.InventoryItemType.Block)
                {
                    for (int i = 0; i < displayed.Count; i++)
                    {
                        if (collectable.blockType == displayed[i])
                        {
                            isinlist = true;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < displayedother.Count; i++)
                    {
                        if (collectable.blockType == displayedother[i])
                        {
                            isinlist = true;
                        }

                    }
                }

                if (!isinlist)
                {
                    if (collectable.inventoryItemType == PlayerData.InventoryItemType.Block)
                        displayed.Add(collectable.blockType);
                    else displayedother.Add(collectable.blockType);
                    if (!collectable.isGem)
                    {
                        switch (collectable.inventoryItemType)
                        {
                            case (PlayerData.InventoryItemType.Block): // 0
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountBlock} = ");
                                break;
                            case (PlayerData.InventoryItemType.BlockBackground): // 1
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountBlockBackground} = ");
                                break;
                            case (PlayerData.InventoryItemType.Seed): // 2
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountSeed} = ");
                                break;
                            case (PlayerData.InventoryItemType.BlockWater): // 3
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountBlockWater} = ");
                                break;
                            case (PlayerData.InventoryItemType.WearableItem): // 4
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountWearableItem} = ");
                                break;
                            case (PlayerData.InventoryItemType.Weapon): // 5
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountWeapon} = ");
                                break;
                            case (PlayerData.InventoryItemType.Throwable): // 6
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountThrowable} = ");
                                break;
                            case (PlayerData.InventoryItemType.Consumable): // 7
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountConsumable} = ");
                                break;
                            case (PlayerData.InventoryItemType.Shard): // 8
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountShard} = ");
                                break;
                            case (PlayerData.InventoryItemType.Blueprint): // 9
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountBlueprint} = ");
                                break;
                            case (PlayerData.InventoryItemType.Familiar): // 10
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountFamiliar} = ");
                                break;
                            case (PlayerData.InventoryItemType.FAMFood): // 11
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountFAMFood} = ");
                                break;
                            case (PlayerData.InventoryItemType.BlockWiring): // 12
                                data.Add($"{collectable.blockType} + {collectable.inventoryItemType} + {amountBlockWiring} = ");
                                break;
                        }
                        num++;
                    }
                    else
                    {

                        data.Add($"Gem+Gem+{amountGem}=");
                        num++;
                    }

                    

                }
                
            }
            return data;
        }


        public static void Hitdown()
        {
            while (true)
            {
                OutgoingMessages.SendHitBlockMessage(KrakMonoBehaviour.localPlayer.currentPlayerBelowMapPoint, Il2CppSystem.DateTime.UtcNow, false);
                Thread.Sleep(150);
            }
        }

        public static string WorldCommand()
        {
            string data;
            try
            {
                Il2CppSystem.DateTime dt = KrakMonoBehaviour.world.GetLockWorldHelper().GetLastActivatedTime();
                Il2CppSystem.DateTime ct = Il2CppSystem.DateTime.UtcNow;
                string owner = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayerWhoOwnsLockName();
                string ownerid = KrakMonoBehaviour.world.GetLockWorldHelper().GetPlayerWhoOwnsLockId();

                int gems = GemCalculator.DisplayGemAmmount();

                dt = dt.AddYears(1);

                string date = $"{dt.Day} {month(dt.Month)} {dt.Year}";
                string cdate = $"{ct.Day} {month(ct.Month)} {ct.Year}";
                data = $"{owner}!{ownerid}!{date}!{cdate}!{gems}!{KrakMonoBehaviour.world.worldName}";
            }
            catch (System.Exception e)
            {
                Krak.Logger.LogError(e);
                int gems = GemCalculator.DisplayGemAmmount();
                data = $"no owner!no owner!no world lock!no world lock!{gems}!{KrakMonoBehaviour.world.worldName}";
            }

            return data;
        }

        public static ulong channel = 1;
        public static string world;
        public static bool gettingWorldAlready = false;
        public static void WorldInfo1()
        {
            world = world.ToUpper();
            Krak.Logger.LogMessage("Warpin to " +world);
            try
            {
                SceneLoader.CheckIfWeCanGoFromWorldToWorld(world, "", null, false, null);
            } catch { }
            
            Thread.Sleep(100);
            Krak.Logger.LogMessage("Starting to wait...");
            do
            {
                Thread.Sleep(10);
                Krak.Logger.LogError("Waiting worldname " + KrakMonoBehaviour.world.worldName + " " + world);
            } 
            while (KrakMonoBehaviour.world.worldName != world);

            Thread.Sleep(50);
            string info = WorldCommand();
            
            Krak.Logger.LogMessage(info);
            
            Krak.Logger.LogMessage("sent to " + channel);
            KrakMonoBehaviour.Write(KrakMonoBehaviour.client.GetStream(), new APIRequest() { opcode = Opcode.World, Data = $"{info}", Channel = channel });
            Krak.Logger.LogMessage("Send packets for world info");
            
            OutgoingMessages.LeaveWorld();
            KrakMonoBehaviour.world.worldName = "^";
            return;


        }

        public static bool CanJoinWorld()
        {
            if (KrakMonoBehaviour.world.worldName == "^")
            {
                return true;
            }
            return false;
        }

        public static void GetGemsInfo1()
        {
            world = world.ToUpper();
            Krak.Logger.LogMessage("Warpin to " + world);
            try
            {
                SceneLoader.CheckIfWeCanGoFromWorldToWorld(world, "", null, false, null);
            }
            catch { }

            Thread.Sleep(100);
            Krak.Logger.LogMessage("Starting to wait...");
            do
            {
                Thread.Sleep(10);
                Krak.Logger.LogError("Waiting worldname " + KrakMonoBehaviour.world.worldName + " " + world);
            }
            while (KrakMonoBehaviour.world.worldName != world);

            Thread.Sleep(50);
            string info = GemCommand(world);

            Krak.Logger.LogMessage(info);

            Krak.Logger.LogMessage("sent to " + channel);
            KrakMonoBehaviour.Write(KrakMonoBehaviour.client.GetStream(), new APIRequest() { opcode = Opcode.Gems, Data = $"{info}", Channel = channel });
            Krak.Logger.LogMessage("Send packets for world info");

            OutgoingMessages.LeaveWorld();
            KrakMonoBehaviour.world.worldName = "^";
            return;


        }

        public static string GemCommand(string WorldName)
        {
            int[] gems = GemCalculator.GemWorldAmount();
            return $"{WorldName};{gems[0]};{gems[1]};{gems[2]};{gems[3]}";
        }
    }
}