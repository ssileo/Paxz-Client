using UnityEngine;
using HarmonyLib;
using WindowsInput;
using WindowsInput.Native;
using BepInEx;
using BepInEx.Logging;
using BepInEx.IL2CPP;
using BasicTypes;
using Il2CppSystem;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using krak.Api;
using System.Threading;
using Il2CppSystem.Threading;
using Kernys.Bson;

namespace krak
{

    [BepInPlugin("krak.bepinex.pw.ur.mom", "krak", "1.0.0.0")]
    public class Krak : BasePlugin
    {
        public static ManualLogSource Logger;

        

        public override void Load()
        {
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<KrakMonoBehaviour>();
            AddComponent<KrakMonoBehaviour>();

            Logger = this.Log;
            var harmony = new Harmony("krak.harmony.pw.ur.mom");
            harmony.PatchAll();

            
        }
    }

    class KrakMonoBehaviour : MonoBehaviour
    {
        public KrakMonoBehaviour(System.IntPtr ptr) : base(ptr) { }
        readonly InputSimulator inputSim = new InputSimulator();
        public static string PLAYER_NONE = "00000000";

        public static string latest_irc_msg = "";

        public static PaxzClient.teleport.Teleport teleport = new PaxzClient.teleport.Teleport();

        public static World world;
        public static WorldController worldController;
        public static Player localPlayer;
        public static PlayerData playerData;
        public static FishingGaugeMinigameUI fishingGaugeMinigameUI;
        public static FishingResultsPopupUI fishingResultsPopupUI;
        public static PortalData portalData;
        public static InventoryControl inventoryControl;
        public static ChatUI chatUI;
        public static PlayerNamesManager namesManager;
        public static TradeOverlayUI tradeOverlayUI;

        public static BasicTypes.Vector2i lastpos = new BasicTypes.Vector2i(0, 0);

        // Auto farm
        public static World.BlockType oldblock = World.BlockType.None;
        public static int hitsRequired;
        public static int hitsLeft;
        private float time = 0.0f;
        public float interpolationPeriod = 0.16f;
        public static bool collect = false; // Ammount of times to break before collecting;
        public static bool randombool69420 = false; 
        public static bool doOnce = false; 
        public static bool tpBack = false; // variable to make autocollect more reliable

        // blinker
        private float blinkertime = 0.0f;
        public float interpolationPeriodforblinker = 0.15f;

        // aimbot for ai
        private float aiaimbottime = 0.0f;
        public float interpolationPeriodforaiaimbottime = 0.17f;


        // testing ticks
        public static int ticks60 = 0;
        public static int ticksfortest = 0;
        public static int ticks300 = 0;

        public static Il2CppSystem.Collections.Generic.List<NetworkPlayer> otherPlayers = NetworkPlayers.otherPlayers;
        
        //UnhollowerBaseLib.Il2CppReferenceArray<WorldItemBase> worldItemBases = world.worldItemsData;
        
        public Rect windowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 500, 250);

        public void OnGUI()
        {
            
            
            if (Globals.showMenu)
                windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)Menu.DoMyWindow, "KRAK menu :)");

            // esps
            if (world != null)
            {
                // nether boss esp
                if (Globals.netherBossESP)
                {
                    UnhollowerBaseLib.Il2CppArrayBase<NetherBossWraithMonoBehaviour> wraithMonoBehaviours = GameObjectPool.FindObjectsOfType<NetherBossWraithMonoBehaviour>();
                    foreach (NetherBossWraithMonoBehaviour current in wraithMonoBehaviours)
                    {

                        // locations
                        Vector2 screenpointplayer = Camera.main.WorldToScreenPoint(localPlayer.currentPlayerPosition);
                        Vector2 screenpointboss = Camera.main.WorldToScreenPoint(current.tempPosition);
                        screenpointboss.y = Screen.height - screenpointboss.y;
                        screenpointplayer.y = Screen.height - screenpointplayer.y;
                        // draw esp
                        Esp.Render.DrawLine(screenpointplayer, screenpointboss, Color.green, 5);

                    }
                }

                // normal aiesp
                if (Globals.aiESP)
                {
                    UnhollowerBaseLib.Il2CppArrayBase<AIEnemyMonoBehaviourBase> aiEnemies = GameObjectPool.FindObjectsOfType<AIEnemyMonoBehaviourBase>();
                    foreach (AIEnemyMonoBehaviourBase enemy in aiEnemies)
                    {
                        // locations
                        Vector2 screenpointplayer = Camera.main.WorldToScreenPoint(localPlayer.currentPlayerPosition);
                        Vector2 screenpointai = Camera.main.WorldToScreenPoint(enemy.tempPosition);
                        screenpointai.y = Screen.height - screenpointai.y;
                        screenpointplayer.y = Screen.height - screenpointplayer.y;
                        // draw esp

                        if (enemy.aiBase.health > 0)
                            Esp.Render.DrawLine(screenpointplayer, screenpointai, Color.red, 1);
                    }
                }

                if (Globals.playerESP)
                {
                    foreach (NetworkPlayer player in otherPlayers)
                    {
                        PlayerData otherp = player.playerScript.myPlayerData;
                        int bytes = otherp.GetByteCoinAmount();
                        
                        
                        // locations
                        Vector2 screenpointplayer = Camera.main.WorldToScreenPoint(localPlayer.currentPlayerPosition);
                        Vector3 screenpointplayer2 = Camera.main.WorldToScreenPoint(player.playerScript.myTransform.position);
                        screenpointplayer2.y = Screen.height - screenpointplayer2.y;
                        screenpointplayer.y = Screen.height - screenpointplayer.y;
                        // draw esp
                        if (world != null)
                            Esp.Render.DrawLine(screenpointplayer, screenpointplayer2, Color.blue, 1.5f);
                        
                        
                    }
                }

                if (Globals.collectablesESP)
                {
                    
                    foreach (CollectableData collectable in KrakMonoBehaviour.world.collectables)
                    {
                        
                        // locations
                        Vector2 screenpointplayer = Camera.main.WorldToScreenPoint(localPlayer.currentPlayerPosition);
                        Vector2 collectablepos = new Vector2(collectable.posX, collectable.posY);
                        Vector2 screenpointcollectable = Camera.main.WorldToScreenPoint(utilities.ConvertMapPointToWorldPoint(collectable.mapPoint));
                        screenpointcollectable.y = Screen.height - screenpointcollectable.y;
                        screenpointplayer.y = Screen.height - screenpointplayer.y;
                        // draw esp


                        Esp.Render.DrawLine(screenpointplayer, screenpointcollectable, Color.cyan, 1.5f);
                    }
                    for(int y = 0; y < world.worldSizeY;y++)
                    {
                        for (int x = 0; x < world.worldSizeX; x++)
                        {
                            World.BlockType blockType = world.GetBlockType(x, y);
                            if (blockType == World.BlockType.GiftBox || blockType == World.BlockType.NetherGiftBox || blockType == World.BlockType.LabGiftBox || blockType == World.BlockType.NetherExit || blockType == World.BlockType.DeepNetherExit)
                            {


                                // get positions
                                Vector2 screenpointplayer = Camera.main.WorldToScreenPoint(localPlayer.currentPlayerPosition);
                                Vector2 boxpoint = utilities.ConvertMapPointToWorldPoint(new Vector2i(x, y));
                                Vector2 screenpointbox = Camera.main.WorldToScreenPoint(boxpoint);
                                screenpointbox.y = Screen.height - screenpointbox.y;
                                screenpointplayer.y = Screen.height - screenpointplayer.y;

                                // render esp lines
                                if (blockType == World.BlockType.DeepNetherExit || blockType == World.BlockType.NetherExit)
                                    Esp.Render.DrawLine(screenpointplayer, screenpointbox, Color.yellow, 2f);
                                else
                                    Esp.Render.DrawLine(screenpointplayer, screenpointbox, Color.magenta, 1.5f);



                            }

                        }
                    }
                }
            }
            
            // health bar
            if (world != null)
            {
                
            }
           
            

            if (world != null)
            {
               /* if (world.GetBlockType(Globals.mapPoint) != World.BlockType.Tree)
                {
                    float mousey = Screen.height - Input.mousePosition.y;
                    Globals.mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Globals.mapPoint = Utils.ConvertWorldPointToMapPoint(Globals.mouseWorldPos);


                    // tile size
                    Globals.tileSize.x = ConfigData.tileSizeX;
                    Globals.tileSize.y = ConfigData.tileSizeY;

                    // positions
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 15, 200, 100), $"Player Pos: {localPlayer.currentPlayerMapPoint}");
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 25, 200, 100), $"Mouse  Pos: {Globals.mapPoint}");
                }
                else

                if (world.GetBlockType(Globals.mapPoint) == World.BlockType.Tree)
                    
                {
                    float mousey = Screen.height - Input.mousePosition.y;
                    seedData = world.GetSeedDataAt(Globals.mapPoint);
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 50, 200, 100), $"pos   : {seedData.position}");
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 60, 200, 100), $"Blocks: {seedData.harvestBlocks}");
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 70, 200, 100), $"Seeds : {seedData.harvestSeeds}");
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 80, 200, 100), $"Gems  : {seedData.harvestGems}");
                    GUI.Label(new Rect(Input.mousePosition.x, mousey + 90, 200, 100), $"Extra : {seedData.harvestExtraBlocks}");
                }
                else
                {
                    
                }
               */
            }



        }
     
        int op = 0;

        public static int tickz = 0;

        public static TcpClient client = new TcpClient();

        public static UnhollowerBaseLib.Il2CppStructArray<PlayerData.InventoryKey> myitems;
        public static UnhollowerBaseLib.Il2CppStructArray<int> amounts;
        
        public void Start()
        {
            
            client.Connect("127.0.0.1", 420);


            Write(client.GetStream(), new APIRequest() { opcode = Opcode.GetServerVersion });
            PlayerData.InventoryKey none;
            none.itemType = ConfigData.GetBlockTypeInventoryItemType(World.BlockType.None);
            none.blockType = World.BlockType.None;
            
            myitems.AddItem(none);
            myitems.AddItem(none);
            myitems.AddItem(none);
            myitems.AddItem(none);
            
            amounts.AddItem(0);
            amounts.AddItem(0);
            amounts.AddItem(0);
            amounts.AddItem(0);
        }

        public void Update()
        {

            NetworkStream stream = client.GetStream();

            if (stream.DataAvailable) // sex
            {

                List<APIRequest> reqs = ReadToEnd(stream);
                ulong channel;
                foreach (var req in reqs)
                {
                    switch (req.opcode)
                    {
                        case Opcode.GetServerVersion:
                            Krak.Logger.LogMessage(req.Data);
                            break;
                        case Opcode.JoinWorld:
                            Krak.Logger.LogMessage(req.Data);
                            SceneLoader.CheckIfWeCanGoFromWorldToWorld(req.Data, "", null, false, null);
                            break;
                        case Opcode.GetGems:
                            int[] k = GemCalculator.GemWorldAmount();
                            Write(client.GetStream(), new APIRequest() { opcode = Opcode.GetGems, Data = $"{k[0]}|{k[1]}|{k[2]}|{k[3]}|{world.worldName}", Channel = req.Channel});
                            break;
                        case Opcode.Restart:
                            SceneLoader.ReloadGame();
                            break;
                        case Opcode.Leave:
                            OutgoingMessages.LeaveWorld();
                            world.worldName = "^";
                            break;
                        case Opcode.LockWorldData:
                            string mk = Methods.LockWorldData4B();
                            channel = req.Channel;
                            Krak.Logger.Log(LogLevel.Message, mk);
                            Write(client.GetStream(), new APIRequest() { opcode = Opcode.LockWorldData, Data = $"{mk}", Channel = channel});
                            break;
                        case Opcode.itemIDs:
                            channel = req.Channel;
                            string iddump = Methods.DumpIDs();
                            Write(client.GetStream(), new APIRequest() { opcode = Opcode.itemIDs, Data = $"{iddump}", Channel= channel });
                            break;
                        case Opcode.Say:
                            Globals.fromDiscord = true;
                            chatUI.Submit(req.Data);
                            break;
                        case Opcode.Move:
                            utilities.Movement(req.Data);
                            break;

                        case Opcode.World:
                            Krak.Logger.LogMessage("Received world data packet");
                            if (world != null)
                            {
                                if (Methods.CanJoinWorld())
                                {
                                    string worldname = req.Data;
                                    Methods.world = worldname;
                                    Methods.channel = req.Channel;
                                    var worldinfo = new Il2CppSystem.Threading.Thread((Il2CppSystem.Threading.ThreadStart)Methods.WorldInfo1);
                                    worldinfo.Start();
                                    
                                }
                                else Write(client.GetStream(), new APIRequest() { opcode = Opcode.World, Data = $"joiningalready", Channel = req.Channel });
                            }
                            else Krak.Logger.LogError("World == null my guy");
                            break;
                        case Opcode.Gems:
                            Krak.Logger.LogMessage("Received gem info packet");
                            if (world != null)
                            {
                                if (Methods.CanJoinWorld())
                                {
                                    string worldname = req.Data;
                                    Methods.world = worldname;
                                    Methods.channel = req.Channel;
                                    var worldinfo = new Il2CppSystem.Threading.Thread((Il2CppSystem.Threading.ThreadStart)Methods.GetGemsInfo1);
                                    worldinfo.Start();

                                }
                                else Write(client.GetStream(), new APIRequest() { opcode = Opcode.Gems, Data = $"joiningalready", Channel = req.Channel });
                            }
                            else Krak.Logger.LogError("World == null my guy");
                            break;
                    }
                }
            }
            else
                System.Threading.Thread.Sleep(1);









            if (Input.GetKeyDown(KeyCode.F5))
            {
                Methods.Scan();
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                SceneLoader.ReloadWorld();
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                SceneLoader.ReloadGame();
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                
                Globals.spamPlaceWL = !Globals.spamPlaceWL;
                Methods.SendMessage($"Spam wl = {Globals.spamPlaceWL}", true);
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                Globals.automath = !Globals.automath;
                Krak.Logger.LogMessage("AutoMath = " + Globals.automath);
                Methods.SendMessage("<#FF00FF>[krak]: <#FFFFFF>AutoMath = " + Globals.automath, false);
            }
           
            if (Input.GetKeyDown(KeyCode.V))
            {
                // Il2CppSystem.Threading.Thread sus = new Il2CppSystem.Threading.Thread((Il2CppSystem.Threading.ThreadStart)Methods.Hitdown);
                // sus.Start();
                
                var sus = new Il2CppSystem.Threading.Thread((Il2CppSystem.Threading.ThreadStart)auto.AutoMine.Start);
                sus.Start();
            }
            

            if (Globals.spamPlaceWL)
            {
                OutgoingMessages.SendSetBlockMessage(Globals.mapPoint/*mappoint of mouse*/, World.BlockType.LockWorld);
            }
            if (Globals.automath)
            {
                tickz++;
            }


            

            // auto farm
            if (world != null)
            {
                
                time += Time.deltaTime;

                if (time >= interpolationPeriod)
                {
                    time -= interpolationPeriod;
                    DateTime hitTime = DateTime.Now;

                    if (Globals.autoFarm)
                    {
                        if (inventoryControl != null) // make sure inventoryControl exists lol
                        {
                            World.BlockType block = Globals.autofarmblock;
                            PlayerData.InventoryItemType type = Globals.autofarmtype;
                            PlayerData.InventoryKey remove = Globals.autofarmremove;

                            Vector3 newTrans;

                            if (collect) // auto Collect
                            {
                                if (tpBack)
                                {
                                    newTrans = utilities.ConvertMapPointToWorldPoint(localPlayer.currentPlayerRightMapPoint);
                                    localPlayer.myTransform.position = new Vector3(newTrans.x, localPlayer.myTransform.position.y, localPlayer.myTransform.position.z);
                                    tpBack = false;
                                }
                                else if (!tpBack)
                                {
                                    if (doOnce)
                                    {
                                        newTrans = utilities.ConvertMapPointToWorldPoint(localPlayer.currentPlayerLeftMapPoint);
                                        localPlayer.myTransform.position = new Vector3(newTrans.x, localPlayer.myTransform.position.y, localPlayer.myTransform.position.z);
                                        collect = false;
                                        tpBack = true;
                                        randombool69420 = false;
                                        hitsLeft = hitsRequired + 2;
                                    }
                                    else
                                    {
                                        doOnce = true;
                                    }

                                    
                                }
                            }
                            else // auto farm logic
                            {
                                
                                
                                if (type == PlayerData.InventoryItemType.Block)
                                {
                                    if (hitsLeft > -1)
                                    {
                                        if (!randombool69420)
                                        {
                                            playerData.RemoveItemsFromInventory(remove, 1);
                                            OutgoingMessages.SendSetBlockMessage(localPlayer.currentPlayerRightMapPoint, block);
                                            randombool69420 = true;


                                        }
                                        else
                                        {
                                            OutgoingMessages.SendHitBlockMessage(localPlayer.currentPlayerRightMapPoint, hitTime, false);
                                            hitsLeft -= 1;
                                        }
                                        
                                    }
                                    else if (hitsLeft == -1)
                                    {
                                        collect = true;
                                    }
                                    
                                }
                                
                            }
                            
                        }
                        else if (inventoryControl == null)
                        {
                            Krak.Logger.LogMessage("[krak]: (Inventory Control == null) Please Relog.");
                        }
                    }



                }
            }
        
            
            // ai aimbot
            if (world != null)
            {
                if (Globals.aiAimbot)
                {
                    aiaimbottime += Time.deltaTime;

                    if (aiaimbottime >= interpolationPeriodforaiaimbottime)
                    {
                        aiaimbottime -= interpolationPeriodforaiaimbottime;
                        UnhollowerBaseLib.Il2CppArrayBase<AIEnemyMonoBehaviourBase> aiEnemies = GameObjectPool.FindObjectsOfType<AIEnemyMonoBehaviourBase>();

                        AIEnemyMonoBehaviourBase closestAiEnemy = null;

                        float shortestDist = 9999999999f;
                        foreach (AIEnemyMonoBehaviourBase current in aiEnemies)
                        {
                            AIEnemyMonoBehaviourBase aiEnemy = current;
                            if (!aiEnemy)
                                continue;
                            float dist = Vector3.Distance(current.tempPosition, localPlayer.currentPlayerPosition);
                            AIEnemyConfigData.GetMaxHitPoints(current.aiBase.enemyType);
                            if (dist < shortestDist && AIEnemyConfigData.GetMaxHitPoints(current.aiBase.enemyType) != 50)
                            {
                                shortestDist = dist;
                                closestAiEnemy = aiEnemy;
                            }

                        }
                        if (closestAiEnemy)
                        {
                            if (closestAiEnemy.aiBase.health > 0)
                                OutgoingMessages.SendHitAIEnemyMessage(utilities.ConvertWorldPointToMapPoint(closestAiEnemy.tempPosition), closestAiEnemy.aiBase.id, 1);

                        }
                    }
                }
            }

            // explode others
            if (world != null)
            {
                if (Globals.muslimMan)
                {

                    if (op > otherPlayers.Count)
                    {
                        op = 0;
                    }
                    else op++;
                    int playercount = otherPlayers.Count;

                    Vector2 diespot = otherPlayers[op].playerScript.transform.position;
                    Vector2i diepoint = utilities.ConvertWorldPointToMapPoint(diespot);
                    localPlayer.isDead = false;
                    if (!localPlayer.isDead)
                    {
                        
                        ChatUI.SendLogMessage($"Blowing up {otherPlayers[op].name}-{op}! playercount = {playercount} ");
                        localPlayer.HitPlayerFromBlock(99999, World.BlockType.SpikeBall, diepoint);
                        
                    }
                }
                
            }

            if (Globals.AdminTools)
                playerData.playerAdminStatus = PlayerData.AdminStatus.AdminStatus_Admin;
            
        
            // infinite jump
            if (Globals.infjump)
            {
                if (world != null)
                {
                    localPlayer.jumpMode = PlayerJumpMode.Double;
                    localPlayer.isDoubleJumpFirstJumpDone = false;
                }
                
            }        

            // open/close menu
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Globals.showMenu = !Globals.showMenu;
            }

            // autofish
            if (Globals.autofish && fishingGaugeMinigameUI != null && fishingResultsPopupUI != null)
            {
                // close popup if it appears
                fishingResultsPopupUI.ClosePopup();

                // auto land fish
                if (fishingGaugeMinigameUI.isReadyToLand)
                {
                    fishingGaugeMinigameUI.LandButtonPressed();
                }

                // auto catch strike
                if (localPlayer.IsFishStrikeActive())
                {
                    inputSim.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                    inputSim.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                }

                // place lure
                if (world.AreMapPointsValidForFishing(localPlayer.currentPlayerMapPoint, localPlayer.currentPlayerMapPoint.x - 1, localPlayer.currentPlayerMapPoint.y - 1) && localPlayer.fishingState == FishingState.NoFishing)
                {
                    BasicTypes.Vector2i playerpos = localPlayer.currentPlayerMapPoint;
                    playerpos.x -= 1;
                    playerpos.y -= 1;
                    PlayerData.InventoryKey[] inventoryData = playerData.GetInventoryAsOrderedByInventoryItemType();
                    for (int i = 0; i < inventoryData.Length; i++)
                    {
                        if (ConfigData.IsFishingLure(inventoryData[i].blockType))
                        {
                            worldController.SetBaitWithTool(inventoryData[i].blockType, playerpos, 0.0f);
                            break;
                        }
                    }
                }

                // no fish runs
                fishingGaugeMinigameUI.fishIsOnTheRun = false;

                // arrow always on fish
                fishingGaugeMinigameUI.targetAreaPosition = fishingGaugeMinigameUI.fishPosition + fishingGaugeMinigameUI.fishVelocity * Time.deltaTime;
                if (fishingGaugeMinigameUI.targetAreaPosition > fishingGaugeMinigameUI.fishTargetPoint)
                {
                    localPlayer.fishingLeftButton = true;
                }
                else
                {
                    localPlayer.fishingRightButton = true;
                }
            }

            // fish hack
            if (Globals.fishHack && !Globals.autofish && fishingGaugeMinigameUI != null)
            {
                // no fish runs
                fishingGaugeMinigameUI.fishIsOnTheRun = false;

                // arrow always on fish
                fishingGaugeMinigameUI.targetAreaPosition = fishingGaugeMinigameUI.fishPosition + fishingGaugeMinigameUI.fishVelocity * Time.deltaTime;
                if (fishingGaugeMinigameUI.targetAreaPosition > fishingGaugeMinigameUI.fishTargetPoint)
                {
                    localPlayer.fishingLeftButton = true;
                }
                else
                {
                    localPlayer.fishingRightButton = true;
                }

            }
        
        
            // auto drop seed
            if (ticks300 == 120)
            {
                if (Globals.autoDropSeed)
                {
                     Auto.AutoDrop.DropSeed(Globals.dropSeedAmmount);
                }

                if (Globals.autoDropBlock)
                {
                     Auto.AutoDrop.DropBlock(Globals.dropSeedAmmount);
                }
                //ChatUI.SendLogMessage($"name: {portalData.name} id: {portalData.entryPointID}, locked = {portalData.isLocked}");
                ticks300 = 0;
                
            }

            // select world background-gravity-effect.
            if (world != null)
            {
                if (
                      world.worldName.ToUpper() != "MINEWORLD" 
                   || world.worldName.ToUpper() != "NETHERWORLD" 
                   || world.worldName.ToUpper() != "JETRACE" 
                   || world.worldName.ToUpper() != "SECRETBASE" 
                   || world.worldName.ToUpper() != "DEEPNETHER"
                   )
                {
                    // select world background
                    if (Globals.CustomBackground)
                    {
                        if (Globals.Background > -1 || Globals.Background < 12)
                        world.worldBackground = (World.LayerBackgroundType)Globals.Background;
                    }
                    // select world weather
                    if (Globals.CustomWeather)
                    {
                        if (Globals.Weather > -1 || Globals.Weather < 13)
                            world.worldWeatherType = (World.WeatherType)Globals.Weather;
                    }
                    

                }
                if (Globals.CustomGravity)
                {
                    if (Globals.Gravity > -1 || Globals.Background < 3)
                        world.gravityMode = (GravityMode)Globals.Gravity;

                    
                }
                
                
            }

            if (Globals.blinker)
            {
                blinkertime += Time.deltaTime;

                if (blinkertime >= interpolationPeriodforblinker)
                {
                    blinkertime -= interpolationPeriodforblinker;
                    int race = new System.Random().Next(1, 15);
                    worldController.player.ChangeSkinColor(race);
                    int countrycodeselect = new System.Random().Next(0, utilities.countrycodes.Length);
                    short countryCode = utilities.countrycodes[countrycodeselect];
                    playerData.countryCode = countryCode;
                    if (playerData.gender == PlayerData.Gender.Male)
                    {
                        playerData.gender = PlayerData.Gender.Female;
                        OutgoingMessages.PlayerInfoUpdated(PlayerData.Gender.Female, countryCode, race, worldController.player.skinColorIndexBeforeSkinColorByWeapon);
                    }

                    else
                    {
                        playerData.gender = PlayerData.Gender.Male;
                        OutgoingMessages.PlayerInfoUpdated(PlayerData.Gender.Male, countryCode, race, worldController.player.skinColorIndexBeforeSkinColorByWeapon);
                    }
                }
            }

            teleport.isTeleporting = false;
            #region teleport
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && Input.GetKey(KeyCode.LeftAlt))
            {
                /* TELEPORT */
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

                Vector2 mouseFull = utilities.toFull(new Vector2(mousePos.x, mousePos.y));
                Vector2i startVec = localPlayer.currentPlayerMapPoint;

                if (Input.GetMouseButton(1))
                {
                    teleport.tryTeleportToTake(startVec, mouseFull);
                    
                }
                else
                    teleport.tryTeleportTo(startVec, mouseFull);
            }
            else if (Input.GetKeyDown(KeyCode.S) && !Globals.inFreeCam && utilities.CanMove())
            {
                /* PLATFORM TELEPORT */
                Vector2i playerVec = localPlayer.currentPlayerMapPoint;

                World.BlockType belowBlock = krak.KrakMonoBehaviour.world.GetBlockType(playerVec.x, playerVec.y - 1);
                if (ConfigData.IsBlockPlatform(belowBlock) || ConfigData.IsBlockInstakill(belowBlock))
                {
                    if (teleport.tryTeleportTo(playerVec, new Vector2i(playerVec.x, playerVec.y - 1)) == PathfindingResult.SUCCESSFUL)
                    {
                        Vector2 mapPoint = utilities.toFloat(new Vector2(playerVec.x, playerVec.y - 1));
                        localPlayer.myTransform.position = new Vector3(mapPoint.x, mapPoint.y, 0);
                    }
                }
            }

            /* DropTeleport */
            /*if (Input.GetKeyDown(KeyCode.Delete) && Globals.itemToolTipEnabled.Value && Globals.itemToolTipItem != World.BlockType.None)
            {
                if (DateTimeOffset.Now.ToUnixTimeSeconds() - Globals.itemToolTipUpdate > 5)
                {
                    Globals.itemToolTipUpdate = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
                else
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                    Vector2 mouseFull = Utils.toFull(new Vector2(mousePos.x, mousePos.y));
                    Vector2i startVec = Paxz.localPlayer.currentPlayerMapPoint;
                    Vector2 startVec1 = new Vector2(startVec.x, startVec.y);

                    if (Vector2.Distance(mouseFull, startVec1) <= 2)
                    {
                        /* Drop at Player Pos 
                        //OutgoingMessages.SendDropItemMessage(new Vector2i((int)mouseFull.x, (int)mouseFull.y), Globals.itemToolTipKey, 1, Paxz.localPlayerData.GetInventoryData(Globals.itemToolTipKey));
                    }
                    else
                    {
                        /* Teleport + Drop 
                        teleport.tryTeleportToTake(startVec, mouseFull, true, true);
                    }
                }*/
            
            #endregion

            // ticks
            ticksfortest++;
           ticks300++;
        }


        private static List<APIRequest> ReadToEnd(NetworkStream stream)
        {
            List<byte> receivedBytes = new List<byte>();
            while (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];

                stream.Read(buffer, 0, buffer.Length);
                receivedBytes.AddRange(buffer);
            }

            receivedBytes.RemoveAll(b => b == 0);

            string[] payloads = Encoding.UTF8.GetString(receivedBytes.ToArray()).Split('`');

            List<APIRequest> requests = new List<APIRequest>();
            foreach (var payload in payloads)
            {
                try
                {
                    requests.Add(JsonConvert.DeserializeObject<APIRequest>(payload));
                }
                catch
                { }
            }
            requests.RemoveAll(r => r == null);
            return requests;
        }

        public static void Write(NetworkStream stream, APIRequest request)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request) + "`");
            stream.Write(data, 0, data.Length);
        }

    }
}
