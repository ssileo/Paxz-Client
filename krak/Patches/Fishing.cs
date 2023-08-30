using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace krak.Patches
{
    [HarmonyPatch(typeof(FishingGaugeMinigameUI), "SetFishVelocity")]
    public static class FishingGaugeMinigameUI_SetFishVelocity
    {
        public static bool Prefix()
        {
            if (Globals.FreezeFish) return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(FishingGaugeMinigameUI), "SetFishPosition")]
    public static class FishingGaugeMinigameUI_SetFishPosition
    {

        public static void Prefix(FishingGaugeMinigameUI __instance)
        {
           KrakMonoBehaviour.fishingGaugeMinigameUI = __instance;
        }
    }

    [HarmonyPatch(typeof(FishingResultsPopupUI), "SetupMenu")]
    public static class FishingResultsPopupUI_SetupMenu
    {
        public static void Prefix(FishingResultsPopupUI __instance)
        {
            KrakMonoBehaviour.fishingResultsPopupUI = __instance;
        }
    }
}
