using Archipelago.MultiClient.Net.Models;
using BepInEx;
using BepInEx.Logging;
using Blingame.Haak;
using HarmonyLib;
using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace HaakRandomizer {

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        Harmony harmony;
        public static ManualLogSource PatchLogger;

        private void Awake() {
            // Plugin startup logic
            PatchLogger = Logger;
            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Connect to AP
            ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection("localhost:38281", "Droppel");
            archipelagoConnection.Connect();

            ArchipelagoConnection.session.Items.ItemReceived += (receivedItemsHelper) => {
                var itemReceivedName = receivedItemsHelper.PeekItemName();

                // ... Handle item receipt here
                Logger.LogInfo($"ItemReceived: {itemReceivedName}");

                SkillTree instance = PersistentSingleton<SkillTree>.Instance;
                switch (itemReceivedName) {
                    case "KeyCard":
                        var keycard = new InventoryItem();
                        keycard.ItemID = "item_keycard0";
                        MMEventManager.TriggerEvent<MMInventoryEvent>(new MMInventoryEvent(MMInventoryEventType.Pick, "MainInventory", keycard, null));
                        break;
                    case "ProgressiveChargeAttack":
                        instance.Unlock(null, true, 25); // 1: Charge Attack
                        instance.Unlock(null, true, 35); // 2: Immediate Charge
                        instance.Unlock(null, true, 37); // 3: Boosted Charge
                        break;
                    case "ProgressiveSwordWaves":
                        instance.Unlock(null, true, 36); // 1: Sword Waves
                        instance.Unlock(null, true, 36); // 1: Longer Sword Waves
                        break;
                    case "ProgressiveDash":
                        instance.Unlock(null, true, 18); // 1: First Dash
                        instance.Unlock(null, true, 28); // 2: Unlimited slide
                        instance.Unlock(null, true, 13); // 3: Directional Dash
                        break;
                    case "Deflection":
                        instance.Unlock(null, true, 11);
                        break;
                    case "ProgressiveHook":
                        instance.Unlock(null, true, 40); // 1: Piercing Hook
                        instance.Unlock(null, true, 38); // 2: Electric Hook
                        instance.Unlock(null, true, 42); // 3: Ultimate Hook
                        break;
                    case "ProgressiveDivingThrust":
                        instance.Unlock(null, true, 31); // 1: Diving Thrust
                        instance.Unlock(null, true, 44); // 1: Power Diving Thrust
                        break;
                    case "Rampage":
                        instance.Unlock(null, true, 32);
                        break;
                    case "ThrustUp":
                        instance.Unlock(null, true, 33);
                        break;
                    case "Glide":
                        instance.Unlock(null, true, 7);
                        break;
                    case "DashSlash":
                        instance.Unlock(null, true, 9);
                        break;
                    case "GasMask":
                        instance.Unlock(null, true, 19);
                        break;
                    case "LifeShard":
                        PersistentSingleton<SkillTree>.Instance.HpFragments.AddFragment(1);
                        break;
                    case "EnergyShard":
                        PersistentSingleton<SkillTree>.Instance.EpFragments.AddFragment(1);
                        break;
                    case "AttackBoost":
                        instance.Unlock(null, true, 8);
                        break;
                    case "Hookspeed":
                        instance.Unlock(null, true, 9);
                        break;
                    case "Hooklength":
                        instance.Unlock(null, true, 3);
                        break;
                    case "CritRate":
                        instance.Unlock(null, true, 10);
                        break;
                    case "FirstAid":
                        instance.Unlock(null, true, 19);
                        break;
                    case "HuntingCert":
                        PersistentSingleton<ColleManager>.Instance.Collect(13, ColleUpdateType.Acquire);
                        break;
                    case "CatFood":
                        PersistentSingleton<ColleManager>.Instance.Collect(85, ColleUpdateType.Acquire);
                        break;
                    case "Dessert Recipe":
                        PersistentSingleton<ColleManager>.Instance.Collect(16, ColleUpdateType.Acquire);
                        break;
                    case "Metal Sniffer":
                        PersistentSingleton<ColleManager>.Instance.Collect(147, ColleUpdateType.Acquire);
                        break;
                    case "East Passcard":
                        PersistentSingleton<ColleManager>.Instance.Collect(123, ColleUpdateType.Acquire);
                        break;
                    case "Disk":
                        PersistentSingleton<ColleManager>.Instance.Collect(5, ColleUpdateType.AcquireAndIncProgress);
                        break;
                    case "DyeYellow":
                        PersistentSingleton<ColleManager>.Instance.Collect(64, ColleUpdateType.Acquire);
                        break;
                    case "DyeRed":
                        PersistentSingleton<ColleManager>.Instance.Collect(65, ColleUpdateType.Acquire);
                        break;
                    case "DyeGreen":
                        PersistentSingleton<ColleManager>.Instance.Collect(72, ColleUpdateType.Acquire);
                        break;
                    case "DyePurple":
                        PersistentSingleton<ColleManager>.Instance.Collect(71, ColleUpdateType.Acquire);
                        break;
                    case "Tape Yellow":
                        PersistentSingleton<ColleManager>.Instance.Collect(69, ColleUpdateType.Acquire);
                        break;
                    case "TVAntenna":
                        PersistentSingleton<ColleManager>.Instance.Collect(63, ColleUpdateType.Acquire);
                        break;
                    case "TVMotherBoard":
                        PersistentSingleton<ColleManager>.Instance.Collect(62, ColleUpdateType.Acquire);
                        break;
                    case "TVPowerCord":
                        PersistentSingleton<ColleManager>.Instance.Collect(61, ColleUpdateType.Acquire);
                        break;
                    case "PortableStereo":
                        PersistentSingleton<ColleManager>.Instance.Collect(70, ColleUpdateType.Acquire);
                        break;
                    case "AlienLaptop":
                        PersistentSingleton<ColleManager>.Instance.Collect(150, ColleUpdateType.Acquire);
                        break;
                    case "PhotoOfGenie":
                        PersistentSingleton<ColleManager>.Instance.Collect(60, ColleUpdateType.Acquire);
                        break;
                    case "TheIntellectualsSpring":
                        PersistentSingleton<ColleManager>.Instance.Collect(58, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#1":
                        PersistentSingleton<ColleManager>.Instance.Collect(73, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#2":
                        PersistentSingleton<ColleManager>.Instance.Collect(74, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#3":
                        PersistentSingleton<ColleManager>.Instance.Collect(75, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#4":
                        PersistentSingleton<ColleManager>.Instance.Collect(76, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#5":
                        PersistentSingleton<ColleManager>.Instance.Collect(77, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#6":
                        PersistentSingleton<ColleManager>.Instance.Collect(78, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#7":
                        PersistentSingleton<ColleManager>.Instance.Collect(79, ColleUpdateType.Acquire);
                        break;
                    case "Newspaper#8":
                        PersistentSingleton<ColleManager>.Instance.Collect(80, ColleUpdateType.Acquire);
                        break;
                    case "EncryptedNote#1":
                        PersistentSingleton<ColleManager>.Instance.Collect(11, ColleUpdateType.Acquire);
                        break;
                    case "EncryptedNote#2":
                        PersistentSingleton<ColleManager>.Instance.Collect(12, ColleUpdateType.Acquire);
                        break;
                    case "EncryptedNote#3":
                        PersistentSingleton<ColleManager>.Instance.Collect(14, ColleUpdateType.Acquire);
                        break;
                    case "SecretManual#1":
                        PersistentSingleton<ColleManager>.Instance.Collect(112, ColleUpdateType.Acquire);
                        break;
                    case "SecretManual#2":
                        PersistentSingleton<ColleManager>.Instance.Collect(113, ColleUpdateType.Acquire);
                        break;
                    case "SecretManual#3":
                        PersistentSingleton<ColleManager>.Instance.Collect(114, ColleUpdateType.Acquire);
                        break;
                    case "Mysterious Flyer #1":
                        PersistentSingleton<ColleManager>.Instance.Collect(83, ColleUpdateType.Acquire);
                        break;
                    case "Mysterious Flyer #2":
                        PersistentSingleton<ColleManager>.Instance.Collect(84, ColleUpdateType.Acquire);
                        break;
                    case "Map of Brewery":
                        PersistentSingleton<ColleManager>.Instance.Collect(15, ColleUpdateType.Acquire);
                        break;
                    case "Brewery Note":
                        PersistentSingleton<ColleManager>.Instance.Collect(17, ColleUpdateType.Acquire);
                        break;
                    case "Report: Multi-Brain Connectors Research":
                        PersistentSingleton<ColleManager>.Instance.Collect(97, ColleUpdateType.Acquire);
                        break;
                    case "Study of Hypnosis":
                        PersistentSingleton<ColleManager>.Instance.Collect(115, ColleUpdateType.Acquire);
                        break;
                    case "Yoga Flyer":
                        PersistentSingleton<ColleManager>.Instance.Collect(4, ColleUpdateType.Acquire);
                        break;
                    case "How to win over Pretty Girls":
                        PersistentSingleton<ColleManager>.Instance.Collect(96, ColleUpdateType.Acquire);
                        break;
                    case "Report: Robot Virus":
                        PersistentSingleton<ColleManager>.Instance.Collect(119, ColleUpdateType.Acquire);
                        break;
                    case "Torn Picture (Left)":
                        PersistentSingleton<ColleManager>.Instance.Collect(93, ColleUpdateType.Acquire);
                        break;
                    case "Torn Picture (Right)":
                        PersistentSingleton<ColleManager>.Instance.Collect(95, ColleUpdateType.Acquire);
                        break;
                    case "Ms Lees Diary":
                        PersistentSingleton<ColleManager>.Instance.Collect(1, ColleUpdateType.Acquire);
                        break;
                }

                receivedItemsHelper.DequeueItem();
            };


            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F5)) {
                GameObject debug = GameObject.Find("UICamera/overlayCanvas/ui_debugConsole/logPanel");
                debug.SetActive(!debug.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.F7)) {
                Logger.LogInfo($"Connectionstatus: {ArchipelagoConnection.session.DataStorage.GetSlotData(ArchipelagoConnection.session.ConnectionInfo.Slot)["endings"].ToString()}");
            }
        }

        private void OnDestroy() {
            if (ArchipelagoConnection.session != null) {
                ArchipelagoConnection.session.Socket.DisconnectAsync();
                ArchipelagoConnection.session = null;
            }
            harmony.UnpatchSelf();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} unloaded!");
        }
    }

    [HarmonyPatch(typeof(PickableItem))]
    public class PickableItemPatch {
        [HarmonyPatch("PickImpl", new Type[] { })]
        private static bool Prefix(PickableItem __instance) {
            HaakTextUgui levelNameObject = GameObject.Find("UICamera/overlayCanvas/ui_mapPanel/ui_minimapPanel/infoPanel/ForegroundImage/Panel/levelName").GetComponent<HaakTextUgui>();

            var mtextField = typeof(HaakTextUgui).GetField("m_text", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            string locationName = (string)mtextField.GetValue(levelNameObject);
            locationName = locationName.ToLower().Replace(' ', '_');
            locationName += "_room_";

            GameObject gameObject = __instance.gameObject;
            Plugin.PatchLogger.LogInfo($"Test: {gameObject.name}");
            if (gameObject.name.Contains("coin")) {
                return true;
            } else if (gameObject.name.Equals("itemDrop_item")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemDrop_item_damageup")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_keycard (2)")) {
                gameObject = gameObject.transform.parent.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_blueVar_leeDairy")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_other_boboRadio")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_photo 1/2 huang")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_blueVar 3kyo note #3")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_blueVar fatty photo")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_blueVar")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_blueVar_eastPass")) {
                gameObject = gameObject.transform.parent.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_B2Breporter")) {
                gameObject = gameObject.transform.parent.parent.gameObject;
            } else if (gameObject.name.Equals("itemPicker_colle_AWlaptop")) {
                gameObject = gameObject.transform.parent.parent.gameObject;
            } else if (gameObject.name.Equals("itempicker_breweryMap")) {
                gameObject = gameObject.transform.parent.gameObject;
            } else if (gameObject.name.Equals("itempicker_riddlenote")) {
                gameObject = gameObject.transform.parent.gameObject;
            }

            // Additional Jumps
            if (gameObject.name.Equals("itemDropSeq_black")) {
                gameObject = gameObject.transform.parent.gameObject;
            }

            // Get roomnumber
            var roomObject = gameObject.transform.parent.parent;
            if (roomObject.name.Contains("New")) {
                locationName += roomObject.name.Substring(8);
            } else {
                locationName += roomObject.name.Split('_')[1];
            }
            locationName += "_";

            string name = gameObject.name;
            if (name.Contains("hp_frag")) {
                locationName += "lifeshard";
            } else if (name.Contains("ep_frag")) {
                locationName += "energyshard";
            } else if (name.Contains("disk")) {
                if (name.Contains("diskVar BT#2") || name.Contains("diskVar 7") || name.Contains("diskVar 5") || name.Contains("diskVar (2) farm BT#1(need glide)")) {
                    locationName += "disk2";
                } else {
                    locationName += "disk";
                }
            } else if (name.Contains("newspaper")) {
                locationName += "newspaper";
            } else if (name.Contains("skill critical")) {
                locationName += "critrate";
            } else if (name.Contains("skill speed")) {
                locationName += "hookspeed";
            } else if (name.Contains("skill length")) {
                locationName += "hooklength";
            } else if (name.Contains("ATTACK+1")) {
                locationName += "attackboost";
            } else if (name.Contains("FirstAidPack")) {
                locationName += "firstaid";
            } else if (name.Contains("black_potion")) {
                locationName += "firstaid";
            } else if (name.Contains("keycard")) {
                locationName += "keycard";
            } else if (name.Contains("BossPingFightDirector")) {
                locationName += "dash";
            } else if (name.Contains("tvPart_antenna")) {
                locationName += "tvantenna";
            } else if (name.Contains("colle_ detecotr")) {
                locationName += "metalsniffer";
            } else if (name.Contains("yoga")) {
                locationName += "flyer";
            } else if (name.Contains("leeDiary")) {
                locationName += "diary";
            } else if (name.Contains("blueVar_PUA")) {
                locationName += "book";
            } else if (name.Contains("radiotape")) {
                locationName += "tape";
            } else if (name.Contains("rotbot virus repotrt")) {
                locationName += "report";
            } else if (name.Contains("tvPart_cable")) {
                locationName += "powercord";
            } else if (name.Contains("tvPart_board")) {
                locationName += "tvmotherboard";
            } else if (name.Contains("boboRadio")) {
                locationName += "stereo";
            } else if (name.Contains("cut_huangFattyPhoto_pickSeq")) {
                locationName += "stereo";
            } else if (name.Contains("immediatelyChaged")) {
                locationName += "instantcharge";
            } else if (name.Contains("hookspinner")) {
                locationName += "ultimatehook";
            } else if (name.Contains("rarako photo")) {
                locationName += "photoofgenie";
            } else if (name.Contains("colle_ dye")) {
                locationName += "dye";
            } else if (name.Contains("black_chargedAttack")) {
                locationName += "chargeattack";
            } else if (name.Contains("black_chargedAttack")) {
                locationName += "chargeattack";
            } else if (name.Contains("acquireNote") || name.Contains("note3Cut")) {
                locationName += "encryptednote";
            } else if (name.Contains("black_deflection")) {
                locationName += "deflection";
            } else if (name.Contains("intellectual")) {
                locationName += "intellectuals_spring";
            } else if (name.Contains("black_penetration")) {
                locationName += "piercing_hook";
            } else if (name.Contains("blueVar_certificate")) {
                locationName += "hunting_cert";
            } else if (name.Contains("black_slash")) {
                locationName += "swordwaves";
            } else if (name.Contains("blackhook+1(BT:UP THRUST)")) {
                locationName += "electrichook";
            } else if (name.Contains("black_dive")) {
                locationName += "divingthrust";
            } else if (name.Contains("black_charged+1")) {
                locationName += "boostedcharge";
            } else if (name.Contains("caatfood")) {
                locationName += "catfood";
            } else if (name.Contains("cut_huangFattyPhoto")) {
                locationName += "picture";
            } else if (name.Contains("eastPass")) {
                locationName += "passcard";
            } else if (name.Contains("gasmask")) {
                locationName += "gasmask";
            } else if (name.Contains("blackglide")) {
                locationName += "glide";
            } else if (name.Contains("kungfur#")) {
                locationName += "secretmanual";
            } else if (name.Contains("B2BReport")) {
                locationName += "report";
            } else if (name.Contains("black_alldirDash")) {
                locationName += "fulldirectiondash";
            } else if (name.Contains("KKflyer#2")) {
                locationName += "flyer";
            } else if (name.Contains("alienware")) {
                locationName += "laptop";
            } else if (name.Contains("催眠术")) { // Google Translate: hypnotism
                locationName += "study";
            } else if (name.Contains("(dashslash)")) {
                locationName += "dashslash";
            } else if (name.Contains("black_upThrust")) {
                locationName += "thrustup";
            } else if (name.Contains("black_sliding4ever")) {
                locationName += "unlimitedslide";
            } else if (name.Contains("acquireColle_cutTemplate_1")) {
                locationName += "map";
            } else if (name.Contains("KKposter#1")) {
                locationName += "flyer";
            } else if (name.Contains("acquireColle_cutTemplate_2")) {
                locationName += "brewerynote";
            } else if (name.Contains("acquireColle_cutTemplate_3")) {
                locationName += "dessertrecipe";
            } else if (name.Contains("black_rage (1)")) {
                locationName += "powerdivingthrust";
            } else if (name.Contains("black_rage")) {
                locationName += "rampage";
            } else if (name.Contains("slashWave+1")) {
                locationName += "swordwaves";
            }

            Plugin.PatchLogger.LogInfo($"Test: {locationName}");
            // Send AP Item

            if (__instance is SkillUnlockItem) {
                ((SkillUnlockItem)__instance).BeginShowOff();
            }

            ArchipelagoConnection.SendLocation(locationName);
            return false;
        }
    }
}
