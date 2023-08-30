using System;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace krak
{
    internal class Menu
    {
        public static string dropseedamt = "999";
        public static string dropblockamt = "999";
        public static string hitreq = "3";

        public static Globals.Tab tab = Globals.Tab.Player;
        public static int ticks300 = 0;
        public static int tick2s300 = 0;
        public static int tick3s300 = 0;


        public static void DoMyWindow(int windowID)
        {
            GUI.color = Color.white;
            GUI.backgroundColor = Color.black;
            // Tab switching options
            if (tab == Globals.Tab.Player) GUI.backgroundColor = Color.white;
            if (GUI.Button(new Rect(5f, 25f, 90, 22), "Player"))
                tab = Globals.Tab.Player;

            GUI.backgroundColor = Color.black;
            if (tab == Globals.Tab.World) GUI.backgroundColor = Color.white;
            if (GUI.Button(new Rect(100f, 25f, 90, 22), "World"))
                tab = Globals.Tab.World;

            GUI.backgroundColor = Color.black;
            if (tab == Globals.Tab.Auto) GUI.backgroundColor = Color.white;
            if (GUI.Button(new Rect(195f, 25f, 90, 22), "Auto"))
                tab = Globals.Tab.Auto;

            GUI.backgroundColor = Color.black;
            if (tab == Globals.Tab.Misc) GUI.backgroundColor = Color.white;
            if (GUI.Button(new Rect(290f, 25f, 90, 22), "Misc"))
                tab = Globals.Tab.Misc;



            switch (tab)
            {
                case Globals.Tab.None:
                    // no tab
                    break;

                case Globals.Tab.Player: // PLAYER TAB

                    GUI.color = Color.white;
                    GUI.backgroundColor = Color.red;

                    if (Globals.godmode) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 50f, 90, 22), "God mode"))
                    {
                        Globals.godmode = !Globals.godmode;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.infjump) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 75f, 90, 22), "inf jump"))
                    {
                        Globals.infjump = !Globals.infjump;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.antibounce) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 100f, 90, 22), "Anti-bounce"))
                    {
                        Globals.antibounce = !Globals.antibounce;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.blinker) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 125f, 90, 22), "Blinker"))
                    {
                        Globals.blinker = !Globals.blinker;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.visuallavahit) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 150f, 90, 22), "V lavahit"))
                    {
                        Globals.visuallavahit = !Globals.visuallavahit;
                    }
                    GUI.backgroundColor = Color.red;
                    
                    if (Globals.aiAimbot) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 175f, 90, 22), "ai aimbot"))
                    {
                        Globals.aiAimbot = !Globals.aiAimbot;
                    }
                    GUI.backgroundColor = Color.red;


                    GUI.backgroundColor = Color.magenta;
                    if (GUI.Button(new Rect(5f, 200f, 90, 22), "fast drop"))
                        Methods.Fastdrop();
                    GUI.backgroundColor = Color.red;

                    if (Globals.antiportal) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(100f, 50f, 90, 22), "Anti Portal"))
                    {
                        Globals.antiportal = !Globals.antiportal;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.antiportal) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(100f, 75f, 90, 22), "Anti Portal"))
                    {
                        Globals.antiportal = !Globals.antiportal;
                    }
                    GUI.backgroundColor = Color.red;

                    break;

                case Globals.Tab.World: // WORLD TAB

                    GUI.color = Color.white;
                    GUI.backgroundColor = Color.red;


                    if (GUI.Button(new Rect(5f, 200f, 240, 25), "lock world data"))
                        Methods.GetWorldData();

                    if (Globals.miningLight) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 50f, 90, 22), "Mining light"))
                    {
                        Globals.miningLight = !Globals.miningLight;
                    }
                    
                    GUI.backgroundColor = Color.magenta;                   
                    if (GUI.Button(new Rect(5f, 75f, 90, 22), "NetherBoss"))
                    {
                        UnhollowerBaseLib.Il2CppArrayBase<NetherBossWraithMonoBehaviour> wraithMonoBehaviours = GameObjectPool.FindObjectsOfType<NetherBossWraithMonoBehaviour>();
                        foreach (NetherBossWraithMonoBehaviour current in wraithMonoBehaviours)
                        {
                            ChatUI.SendLogMessage($"[krak] Boss at {utilities.ConvertWorldPointToMapPoint(current.tempPosition)}, Health: {current.aiBase.health}/10000");
                        }
                    }

                    if (GUI.Button(new Rect(100f, 75f, 90, 22), "next nether"))
                    {
                        SceneLoader.CheckIfWeCanGoFromWorldToWorld("netherworld", "", null, false, null);
                    }
                    GUI.backgroundColor = Color.red;

                    break;


                case Globals.Tab.Auto: // Auto TAB

                    GUI.color = Color.white;
                    GUI.backgroundColor = Color.red;

                    if (Globals.autofish) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 50f, 90, 22), "auto fish"))
                    {
                        Globals.fishHack = !Globals.fishHack;
                        Globals.autofish = !Globals.autofish;
                    }
                    GUI.backgroundColor = Color.red;

                    

                    if (Globals.autoDropSeed) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 75f, 90, 22), "-Seed Drop"))
                    {
                        Globals.autoDropSeed = !Globals.autoDropSeed;
                    }
                    GUI.backgroundColor = Color.black;             
                    dropseedamt = GUI.TextField(new Rect(100f, 75f, 90, 22), dropseedamt);
                    try
                    {
                        Globals.dropSeedAmmount = System.Convert.ToInt32(dropseedamt);
                    }
                    catch
                    {
                        // doesnt needa do nothing
                        if (ticks300 == 300)
                        {
                            ChatUI.SendLogMessage("invalid entry");
                            ticks300 = 0;
                        }
                        else
                        {
                            ticks300++;
                        }

                    }
                    //pmg
                    GUI.backgroundColor = Color.red;
                    if (Globals.autoDropBlock) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 100f, 90, 22), "-Block Drop"))
                    {
                        Globals.autoDropBlock = !Globals.autoDropBlock;
                    }
                    GUI.backgroundColor = Color.black;
                    dropblockamt = GUI.TextField(new Rect(100f, 100f, 90, 22), dropblockamt);
                    try
                    {
                        Globals.dropBlockAmmount = System.Convert.ToInt32(dropblockamt);
                    }
                    catch
                    {
                        // doesnt needa do nothing
                        if (tick2s300 == 300)
                        {
                            ChatUI.SendLogMessage("invalid entry");
                            tick2s300 = 0;
                        }
                        else
                        {
                            tick2s300++;
                        }
                    }

                    // Auto farm
                    GUI.backgroundColor = Color.red;
                    if (Globals.autoFarm) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 125f, 90, 22), "auto farm"))
                    {
                        Globals.autoFarm = !Globals.autoFarm;
                        KrakMonoBehaviour.collect = false;
                        KrakMonoBehaviour.tpBack = false;
                        KrakMonoBehaviour.randombool69420 = false;
                        KrakMonoBehaviour.doOnce = true;
                        KrakMonoBehaviour.hitsLeft = KrakMonoBehaviour.hitsRequired;
                    }
                    
                    GUI.backgroundColor = Color.black;
                    hitreq = GUI.TextField(new Rect(100f, 125f, 90, 22), hitreq);
                    try
                    {
                        KrakMonoBehaviour.hitsRequired = System.Convert.ToInt32(hitreq);
                    }
                    catch
                    {
                        // doesnt needa do nothing
                        if (tick3s300 == 300)
                        {
                            ChatUI.SendLogMessage("invalid entry for autofarm");
                            tick3s300 = 0;
                        }
                        else
                        {
                            tick3s300++;
                        }
                    }
                    GUI.backgroundColor = Color.red;



                    break;

                case Globals.Tab.Misc: // misc TAB

                    GUI.color = Color.white;
                    GUI.backgroundColor = Color.red;

                    if (Globals.FreezeFish) GUI.backgroundColor = Color.green;                  
                    if (GUI.Button(new Rect(5f, 50f, 90, 22), "Freeze-fish"))
                    {
                        Globals.FreezeFish = !Globals.FreezeFish;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.spamPlaceWL) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 75f, 90, 22), "spam wl"))
                    {
                        Globals.spamPlaceWL = !Globals.spamPlaceWL;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.playerESP) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 100f, 90, 22), "player esp"))
                    {
                        Globals.playerESP = !Globals.playerESP;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.collectablesESP) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 125f, 90, 22), "collectable esp"))
                    {
                        Globals.collectablesESP = !Globals.collectablesESP;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.aiESP) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(5f, 150f, 90, 22), "ai esp"))
                    {
                        Globals.aiESP = !Globals.aiESP;
                    }
                    GUI.backgroundColor = Color.red;

                    if (Globals.AdminTools) GUI.backgroundColor = Color.green;
                    if (GUI.Button(new Rect(100f, 50f, 90, 22), "Admin Tools"))
                    {
                        Globals.AdminTools = !Globals.AdminTools;
                        if (Globals.AdminTools) ChatUI.SendMinigameMessage("Admin Tools are enabled becareful!");
                        else if (!Globals.AdminTools) ChatUI.SendMinigameMessage("Disabled Admin Tools");
                    }


                    

                    break;
            }

            GUI.DragWindow();
        }

    }
}
