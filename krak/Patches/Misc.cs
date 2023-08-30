using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace krak.Patches
{

    [HarmonyPatch(typeof(PlayerNamesManager), "Start")]
    public static class PlayerNamesManager_Start
    {
        public static void Prefix(PlayerNamesManager __instance) { KrakMonoBehaviour.namesManager = __instance; }
    }


    [HarmonyPatch(typeof(OutgoingMessages), "AddMapPointIfNotLastAlready")]
    public static class OutgoingMessages_AddMapPointIfNotLastAlready
    {
        public static bool Prefix()
        {
            return !KrakMonoBehaviour.teleport.isTeleporting;
        }
    }

    

    

    [HarmonyPatch(typeof(OutgoingMessages), "UpdateMyTradeItems")]
    public static class OutgoingMessages_UpdateMyTradeItems
    {
        public static bool Prefix()
        {
            
            return !KrakMonoBehaviour.teleport.isTeleporting;
        }
    }
}
