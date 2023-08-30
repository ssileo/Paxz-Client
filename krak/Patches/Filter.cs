using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace krak.Patches
{
    [HarmonyPatch(typeof(ProfanityFilter), "Censor")]
    public static class ProfanityFilter_Censor
    {
        public static bool Prefix(ref string __result,ref string str)
        {

            __result = str;
            return false;

        }
    }
}
