using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace krak.Patches
{
    [HarmonyPatch(typeof(ConfigData), "IsBlockHot")]
    public static class ConfigData_IsBlockHot
    {
        public static bool Prefix(ref World.BlockType blockType)
        {
            if (Globals.visuallavahit) blockType = World.BlockType.Lava;
            if (Globals.antibounce) return false;
            // code to run here
            return true;
        }
    }
    [HarmonyPatch(typeof(ConfigData), "IsBlockWind")]
    public static class ConfigData_IsBlockWind
    {
        public static bool Prefix()
        {
            if (Globals.antibounce) return false;
            // code to run here
            return true;
        }
    }
    [HarmonyPatch(typeof(ConfigData), "IsBlockElastic")]
    public static class ConfigData_IsBlockElastic
    {
        public static bool Prefix()
        {
            if (Globals.antibounce) return false;
            // code to run here
            return true;
        }
    }
    [HarmonyPatch(typeof(ConfigData), "IsBlockSpring")]
    public static class ConfigData_IsBlockSpring
    {
        public static bool Prefix()
        {
            if (Globals.antibounce) return false;
            // code to run here
            return true;
        }
    }
    
    

    [HarmonyPatch(typeof(ConfigData), "GetBlockRunSpeed")]
    public static class ConfigData_GetBlockRunSpeed
    {
        public static bool Prefix(ref float __result)
        {
            if (Globals.customRunSpeed)
            {
                __result = Globals.runSpeed;
                return false;
            }
            return true;
        }
    }
    


}
