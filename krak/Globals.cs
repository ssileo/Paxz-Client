using UnityEngine;
using BasicTypes;
using System.Collections.Generic;

namespace krak
{
    internal class Globals
    {

        public enum Tab
        {
            None,
            Player,
            World,
            Auto,
            Misc
        }

        public static bool fromDiscord = false;

        public static Vector2 mouseWorldPos;
        public static Vector2 tileSize = new Vector2(ConfigData.tileSizeX, ConfigData.tileSizeY);
        public static Vector2i mapPoint;

        public static bool showMenu = false;
        public static bool debugMouse = false;
        //esp bools
        public static bool aiESP = false;
        public static bool netherBossESP = false;
        public static bool playerESP = false;
        public static bool collectablesESP = false;
        
        public static bool autoDropSeed = false;
        public static int dropSeedAmmount = 999;
        public static bool autoDropBlock = false;
        public static int dropBlockAmmount = 999;

        // auto farm
        public static bool autoFarm = false;
        public static int hitreq = 5;
        public static PlayerData.InventoryItemType autofarmtype;
        public static World.BlockType autofarmblock;
        public static PlayerData.InventoryKey autofarmremove;

        public static bool customRunSpeed = false;
        public static float runSpeed = 10f;

              
        public static bool antibounce = true;
        public static bool antiportal = true;
        public static bool FreezeFish = false;
        public static bool infjump = false;
        public static bool miningLight = true;
        public static bool godmode = true;
        public static bool antiSwear = false;
        public static bool blinker = false;
        public static bool muslimMan = false;
        public static bool visuallavahit = false;
        public static bool visuallonghit = false;
        public static bool aiAimbot = false;
        public static bool FreeCam = false;
        public static bool spamPlaceWL = false;
        public static bool automath = false;

        public static bool autofish = false;
        public static bool fishHack = false;

        
        public static bool AdminTools = false;
        public static bool modTools = false;

        public static bool inFreeCam = false;

        
        public static bool CustomBackground = false;
        public static bool CustomWeather = false;
        public static bool CustomGravity = false;

        // int bools 
        public static int Background = 0;
        public static int Weather = 0;
        public static int Gravity = 0;

        // auto farm
        public static int[,] AutoLocations =
        {
                {-2, -1, 0, 1, 2},
                {-2, -1, 0, 1, 2},
                {-2, -1, 0, 1, 2},
                {-2, -1, 0, 1, 2},
                {-2, -1, 0, 1, 2}
        };
        public static bool[,] AutoLocationToggle =
        {
            {false, false, false, false, false },
            {false, false, false, false, false },
            {false, false, false, false, false },
            {false, false, false, false, false },
            {false, false, false, false, false }
        };


        public static Dictionary<string, string> staff = new Dictionary<string, string>()
        {
			// goos
			{"8TC5JJCR", "Gooooose"},
			// mods
			{"DY4LVBNE", "BlackWight"},
            {"1BYM5371", "Rabia"},
            {"VSL1HVDO", "Citrina"},
            {"LKB469T7", "Starfire1178"},
            {"95F6JEWA", "ionas"},
            {"I501W0UX", "xSHANEx"},
            {"60FPOJ55", "Invalid"},
            {"VZ7RALO", "ColdUnwanted"},
            {"LNBJ9SK", "Quqqo"},
            {"IRIME6M", "Lupuss"},
            {"SAZUT30", "Freak"},
            {"OMD5ECO", "Hinter"},
            {"2SYJQ2R", "SEAF"},
            {"5V6MYXQ3", "NicoKapell"},
            {"G2D8FGE", "MrGrandman"},
            {"48WESIAE", "|Serxan|"},
            {"LSPMU8CV", "[MOD_CADET]"},
            {"WUAOAQ4T", "Luucsas"},
			// admins
			{"34N8P51", "Jake"},
            {"HN55GSS", "EndlesS"},
            {"74RL1AE", "MidnightWalker"},
            {"8HN45WF", "Dev"},
            {"F2RQK1W", "Commander_K"},
            {"NZRV2SD", "Sorsa" },
            {"FAL8MX5Y", "Siskea" }
        };

    }
}
