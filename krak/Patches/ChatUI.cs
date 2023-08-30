using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace krak.Patches
{
    [HarmonyPatch(typeof(ChatUI), "Submit")]
    public static class ChatUI_Submit
    {
        public static bool Prefix(ref string text)
        {
            
            string[] textt = text.Split(' ');
            
            // background selecter
            if (textt[0] == "/bg")
            {
                if (textt[1] != null)
                {
                    Globals.CustomBackground = true;
                    Globals.Background = Convert.ToInt32(textt[1]);
                    ChatUI.SendLogMessage($"Background {Globals.Background} selected.");
                    return false;
                }
                else if (textt[1] == "none")
                {
                    Globals.CustomBackground = false;
                    return false;
                }



            }

            if (textt[0] == "/sell")
            {
                Methods.SellGems();
                return false;
            }
            if (textt[0] == "/reload")
            {
                SceneLoader.CheckIfWeCanGoFromWorldToWorld(KrakMonoBehaviour.world.worldName, "", null, false, null);
                return false;
            }
            if (textt[0] == "/nuke")
            {
                Methods.Penis();
                return false;
            }
            if (textt[0] == "/block")
            {
                Methods.BlockAll(System.Convert.ToInt32(textt[1]));
                return false;
            }
            if (textt[0] == "/kill" || textt[0] == "/kys")
            {
                Methods.CustomDeath(System.Convert.ToInt32(textt[1]));
                return false;
            }
            if (textt[0] == "/break")
            {
                return false;
            }
            if (textt[0] == "/get")
            {
                //Methods.Get(System.Convert.ToInt32(textt[1]));
                return false;
            }
            if (textt[0] == "/select")
            {
                Globals.autofarmblock = KrakMonoBehaviour.inventoryControl.GetCurrentSelection().blockType;
                Globals.autofarmtype = KrakMonoBehaviour.inventoryControl.GetCurrentSelection().itemType;
                Globals.autofarmremove = KrakMonoBehaviour.inventoryControl.GetCurrentSelection();
                return false;
            }
            if (textt[0] == "/add")
            {
                int[] amounts = {0,0,0,0};
                PlayerData.InventoryKey none;
                none.itemType = ConfigData.GetBlockTypeInventoryItemType(World.BlockType.None);
                none.blockType = World.BlockType.None;
                PlayerData.InventoryKey[] iks = { none, none, none, none };
                //OutgoingMessages.UpdateMyTradeItems(100, iks, amounts, false, false);
                TradeOverlayUI.myBC = 100;
                return false;
            }





            bool okay = Methods.Warp(text);
            if (!okay)
                return false;
            bool okayy = Methods.Drop(text);
            if (!okayy)
                return false;
            bool okayyy = Methods.BytesOrGems(text);
            if (!okayyy)
                return false;

            // help
            if (textt[0] == "/help")
            {
                ChatUI.SendMinigameMessage
                    (
                    "[krak]" +
                    " /w or " +
                    " /warp {worldname} ; " +
                    " /drop {ammount} ; " +
                    " /sell ; /bg {id} ; " +
                    " /gems {ammount} ; " +
                    " /bytes {ammount} ; " +
                    " /xp {ammount} ;" +
                    " /reload"
                    );
                return false;
            }
            return !PaxzClient.commands.CommandExecutor.Execute(text);
        }
    }

    [HarmonyPatch(typeof(ChatMessage), "StripTags")]
    public static class ChatMessage_Striptags
    {
        public static bool Prefix(ref bool __result)
        {
            if (Globals.fromDiscord)
            {
                Globals.fromDiscord = false;
                return true;
            }

            __result = true;
            return false;


        }
        
    }

    [HarmonyPatch(typeof(ChatUI), "Start")]
    public static class ChatUI_Start
    {
        public static void Prefix(ChatUI __instance)
        {
            KrakMonoBehaviour.chatUI = __instance;
        }
        public static void Postfix()
        {
            KrakMonoBehaviour.chatUI.chatPointerGesture.tapAndHoldThreshold = 0.15f;
        }
    }

    
    [HarmonyPatch(typeof(ChatUI), "NewMessage")]
    public static class ChatUI_NewMessage
    {
        public static bool AntiDC;
        public static void Prefix(ref ChatMessage msg)
        {
            if (Globals.automath && AntiDC)
            {
                if (msg.channelType == 0)
                {
                    try
                    {
                        string nox = msg.message.Replace('x', '*');
                        double sum = Methods.Evaluate(nox);
                        if (sum.ToString() == "NaN")
                        {
                            Krak.Logger.LogMessage("[krak] NaN value for double ignored.");
                        }
                        else
                        {
                            KrakMonoBehaviour.chatUI.Submit(sum.ToString());
                        }
                        string apples = $"[krak] Did math sum answer = {Methods.Evaluate(msg.message)}";
                        Krak.Logger.LogMessage(apples);
                        AntiDC = false;
                    }
                    catch
                    {
                        string apples4 = $"[krak] {msg.nick}: {msg.message}";
                        Krak.Logger.LogMessage(apples4);
                        AntiDC = false;
                    }
                    
                }
            }
            else if (Globals.automath && !AntiDC)
            {
                AntiDC = true;
            }

           
        }
        
    }



}
