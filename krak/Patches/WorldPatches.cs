using HarmonyLib;
using krak;
using UnityEngine;
namespace krak.Patches
{

    [HarmonyPatch(typeof(World), "GenerateLayersFromBSON")]
    public static class World_GenerateLayersFromBSON
    {
        public static void Prefix(World __instance)
        {
            KrakMonoBehaviour.world = __instance;
        }
    }

    [HarmonyPatch(typeof(WorldController), "Start")]
    public static class WorldController_Start
    {
        public static void Prefix(WorldController __instance)
        {
            KrakMonoBehaviour.worldController = __instance;
        }
    }

    [HarmonyPatch(typeof(WorldController), "InstantiateFogOfWar")]
    public static class WorldController_InstantiateFogOfWar
    {
        public static bool Prefix()
        {
            if (Globals.miningLight) return false;
            return true;
        }
    }
    [HarmonyPatch(typeof(RootUI), "SetWorldLighting")]
    public static class RootUI_SetWorldLighting
    {
        public static bool Prefix()
        {
            if (Globals.miningLight) return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(World), "CheckShouldTrapBeOn")]
    public static class World_CheckShouldTrapBeOn
    {
        public static bool Prefix()
        {
            //if (Globals.godmode) return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(World), "IsMapPointNearEnough")]
    public static class World_IsMapPointNearEnough
    {
        public static bool Prefix(ref bool __result)
        {
            if (Globals.visuallonghit)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(TrapProjectile), "OnTriggerEnter2D")]
    public static class TrapProjectile_OnTriggerEnter2D
    {
        public static bool Prefix()
        {
            
            return false;
        }
    }

    [HarmonyPatch(typeof(PortalData), "SetViaBSON")]
    public static class PortalData_SetViaBSON
    {
        public static void Prefix(PortalData __instance)
        {
            if (KrakMonoBehaviour.world != null)
            KrakMonoBehaviour.portalData = __instance;
        }
    }
    
}
