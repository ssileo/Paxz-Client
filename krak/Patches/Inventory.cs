using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace krak.Patches
{
    [HarmonyPatch(typeof(InventoryControl), "Setup")]
    public static class InventoryControl_Setup
    {
        public static bool Prefix(InventoryControl __instance)
        {
            if (KrakMonoBehaviour.world != null)
            KrakMonoBehaviour.inventoryControl = __instance;
            return true;
        }
    }

    [HarmonyPatch(typeof(MainMenuPersonal), "LogoutButtonClicked")]
    public static class MainMenuPersonal_LogoutButtonClicked
    {
        public static bool Prefix()
        {
            UserIdent.LogOut();
            SceneLoader.ReloadGame();
            return false;
        }
    }
}
