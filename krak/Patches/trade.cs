using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace krak.Patches
{
    class trade
    {
        [HarmonyPatch(typeof(TradeUIUtility), "PlayerRequestedTrade")]
        public static class TradeUIUtility_PlayerRequestedTrade
        {
            public static void Prefix(ref string starterID)
            {
                Krak.Logger.Log(BepInEx.Logging.LogLevel.Message, starterID);
            }
        }
        [HarmonyPatch(typeof(World), "DoesWorldContainUntradeableBlocks")]
        public static class World_DoesWorldContainUntradeableBlocks
        {
            public static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(TradeOverlayUI), "Update")]
        public static class TradeOverlayUI_Update
        {
            public static bool Prefix(TradeOverlayUI __instance)
            {
                KrakMonoBehaviour.tradeOverlayUI = __instance;
                return true;
            }
        }
        
    }
}
