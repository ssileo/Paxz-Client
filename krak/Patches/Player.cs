using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicTypes;
using HarmonyLib;
namespace krak.Patches
{
    [HarmonyPatch(typeof(PlayerData), "AddListenerForVIPAmountChanged")]
    public static class PlayerData_AddListenerForVIPAmountChanged
    {
        public static void Prefix(PlayerData __instance)
        {
            KrakMonoBehaviour.playerData = __instance;
        }
    }

    [HarmonyPatch(typeof(Player), "Awake")]
    public static class Player_Awake
    {
        public static void Prefix(Player __instance)
        {
            if (KrakMonoBehaviour.playerData == __instance.myPlayerData)
            {
                KrakMonoBehaviour.localPlayer = __instance;
            }
        }
    }

    [HarmonyPatch(typeof(Player), "HitPlayerFromAIEnemy", new System.Type[] { typeof(AIBase), typeof(AIDamageModelType) })]
    public static class Player_HitPlayerFromAIEnemy1
    {
        public static bool Prefix()
        {
            if (Globals.godmode) return false;//KrakMonoBehaviour.localPlayer.TakeHitFromOtherPlayer("00000000", false, true, World.BlockType.SoilBlock, 0, 100.0f);
            return !Globals.godmode;
        }
    }

    [HarmonyPatch(typeof(Player), "HitPlayerFromBlock", new System.Type[] { typeof(int), typeof(World.BlockType), typeof(Vector2i) })]
    public static class Player_HitPlayerFromBlock1
    {
        public static bool Prefix(ref bool __result, World.BlockType hitFromBlockBlockType, Vector2i blockMapPoint )
        {
            if (Globals.godmode)
            {
                
                OutgoingMessages.SendPlayPlayerAudioMessage(14, ((int)hitFromBlockBlockType));
                return true;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Player), "HitPlayerFromBlock", new System.Type[] { typeof(World.BlockType), typeof(Vector2i), typeof(bool) })]
    public static class Player_HitPlayerFromBlock2
    {
        public static bool Prefix(ref bool __result,World.BlockType blocktype, Vector2i blockMapPoint )
        {
            if (Globals.godmode)
            {

                //OutgoingMessages.SendPlayPlayerAudioMessage(14, ((int)blocktype));
                OutgoingMessages.SendPlayPlayerAudioMessage(14, ((int)blocktype));
                return true
                /*KrakMonoBehaviour.localPlayer.HitPlayerFromBlock(blocktype, blockMapPoint, false)*/;
                //return KrakMonoBehaviour.localPlayer.HitPlayerFromBlock(blockType, KrakMonoBehaviour.localPlayer.currentPlayerMapPoint, false);
            }
            return true;
        }
    }


    [HarmonyPatch(typeof(Player), "CheckPortals")]
    public static class Player_CheckPortals
    {
        public static bool Prefix()
        {
            if (Globals.antiportal)
            {
                return false;
            }
            return true;
        }
    }


}
