using Archipelago.MultiClient.Net.Helpers;
using BepInEx;
using BepInEx.Logging;
using Blingame.Haak;
using HarmonyLib;
using Luxko.SaveLoad;
using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HaakRandomizer {

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        Harmony harmony;
        public static ManualLogSource PatchLogger;
        public static ItemStorage itemStorage;

        private void Awake() {
            // Plugin startup logic
            PatchLogger = Logger;
            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());


            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void HandleItem(ReceivedItemsHelper receivedItemsHelper) {
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
    [HarmonyPatch(typeof(AcquireHookDirector))]
    public class AcquireHookDirectorPatch {

        [HarmonyPatch("UnlockHook", new Type[] { })]
        private static bool Prefix() {
            ArchipelagoConnection.SendLocation("glove");
            GameManager.Instance.SaveGame(SLContext.Type.PerRun, null, null, GameSaver1.SaveScope.All);
            return false;
        }
    }

    [HarmonyPatch(typeof(PickableItem))]
    public class PickableItemPatch {

        static Dictionary<int, string> itemMappings = new Dictionary<int, string> {
            { 13, "hunting_cert" },
            { 85, "catfood" },
            { 16, "dessertrecipe" },
            { 147, "metalsniffer" },
            { 123, "passcard" },
            { 5, "disk" },
            { 64, "dye" },
            { 65, "dye" },
            { 72, "dye" },
            { 71, "dye" },
            { 66, "dye" },
            { 68, "tape" },
            { 67, "tape" },
            { 69, "tape" },
            { 63, "tvantenna" },
            { 62, "tvmotherboard" },
            { 61, "powercord" },
            { 70, "stereo" },
            { 150, "laptop" },
            { 60, "photo" },
            { 58, "intellectuals_spring" },
            { 73, "newspaper" },
            { 74, "newspaper" },
            { 75, "newspaper" },
            { 76, "newspaper" },
            { 77, "newspaper" },
            { 78, "newspaper" },
            { 79, "newspaper" },
            { 80, "newspaper" },
            { 81, "newspaper" },
            { 82, "newspaper" },
            { 11, "encryptednote" },
            { 12, "encryptednote" },
            { 14, "encryptednote" },
            { 112, "secretmanual" },
            { 113, "secretmanual" },
            { 114, "secretmanual" },
            { 105, "devlog" },
            { 149, "devlog" },
            { 83, "flyer" },
            { 84, "flyer" },
            { 15, "map" },
            { 17, "note" },
            { 97, "report" },
            { 115, "study" },
            { 94, "poster" },
            { 118, "manual" },
            { 148, "photo" },
            { 4, "flyer" },
            { 96, "book" },
            { 119, "report" },
            { 93, "picture" },
            { 95, "picture" },
            { 117, "letter" },
            { 98, "report" },
            { 1, "diary" },
        };

        static Dictionary<int, string> skillMappings = new Dictionary<int, string>() {
            { 1, "glove" },
            { 25, "chargeattack" },
            { 35, "instantcharge" },
            { 37, "boostedcharge" },
            { 36, "swordwaves" },
            { 18, "dash" },
            { 28, "unlimitedslide" },
            { 13, "fulldirectiondash" },
            { 12, "dashdamage" },
            { 11, "deflection" },
            { 40, "piercinghook" },
            { 38, "electrichook" },
            { 42, "ultimatehook" },
            { 31, "divingthrust" },
            { 44, "powerdivingthrust" },
            { 32, "rampage" },
            { 33, "thrustup" },
            { 7, "glide" },
            { 9, "dashslash" },
            { 19, "gasmask" },
            { 41, "hacker" },
        };

        [HarmonyPatch("PickImpl", new Type[] { })]
        private static bool Prefix(PickableItem __instance) {
            HaakTextUgui levelNameObject = GameObject.Find("UICamera/overlayCanvas/ui_mapPanel/ui_minimapPanel/infoPanel/ForegroundImage/Panel/levelName").GetComponent<HaakTextUgui>();

            var mtextField = typeof(HaakTextUgui).GetField("m_text", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            string levelName = (string)mtextField.GetValue(levelNameObject);
            string locationName = levelName;
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

            ColleOnPick collOnPick = gameObject.GetComponent<ColleOnPick>();
            SkillUnlockItem skillUnlock = gameObject.GetComponent<SkillUnlockItem>();
            SkillFragmentOnPick skillFragment = gameObject.GetComponent<SkillFragmentOnPick>();
            ItemPicker itemPicker = gameObject.GetComponent<ItemPicker>();
            if (collOnPick != null) {
                if (collOnPick.colleEvent.Index == 5) {
                    // Two disks in same room
                    var mTrackID = typeof(ColleOnPick).GetField("_trackId", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                    int trackID = (int) mTrackID.GetValue(collOnPick);
                    if (trackID == 1674193335 || trackID == 903607979 || trackID == 1789000584 || trackID == 482506121) {
                        locationName += "disk2";
                    } else {
                        locationName += "disk";
                    }
                } else {
                    locationName += itemMappings[collOnPick.colleEvent.Index];
                }
            } else if (skillUnlock != null) {
                locationName += skillMappings[skillUnlock.skillIndex];
            } else if (skillFragment != null) {
                locationName += skillFragment.FragmentID == FragmentedSkill.Type.Health ? "lifeshard" : "energyshard";
            } else if (itemPicker != null) {
                locationName += "keycard";
            } else {
                Plugin.PatchLogger.LogInfo("Unknown Item Type");
                foreach (Component c in gameObject.GetComponents(typeof(Component))) {
                    Plugin.PatchLogger.LogInfo($"Component: {c.GetType()}");
                }
                return false;
            }

            // For Skills do showoff, otherwise we softlock
            if (__instance is SkillUnlockItem) {
                ((SkillUnlockItem)__instance).BeginShowOff();
            }

            // Send AP Item
            Plugin.PatchLogger.LogInfo($"Test: {locationName}");
            ArchipelagoConnection.SendLocation(locationName);
            if (__instance.SaveOnPick) {
                GameManager.Instance.SaveGame(__instance.SaveOnPickContext, null, null, GameSaver1.SaveScope.All);
            }
            return false;
        }


    }

    [HarmonyPatch(typeof(GameManager))]
    public class GameManagerPatch {
        [HarmonyPostfix]
        [HarmonyPatch("SaveGame", new Type[] { typeof(SLContext.Type), typeof(bool), typeof(Vector2), typeof(GameSaver1.SaveScope) })]
        private static void SaveGamePost() {
            if (Plugin.itemStorage == null) {
                return;
            }
            Plugin.itemStorage.StoreFile();
            return;
        }

        [HarmonyPostfix]
        [HarmonyPatch("TitleScreenRespawnFade", new Type[] { typeof(GameManager.LoadContext1) })]
        private static void TitleScreenRespawnFadePost(GameManager __instance) {
            Plugin.PatchLogger.LogInfo(GameManager._platformMgr.ActiveSaverIndex);
            //try {
            //    var saveSlotField = typeof(GameManager).GetField("_activeSaverIndex", BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance);
            //    Plugin.PatchLogger.LogInfo(saveSlotField.GetValue(saveSlotField));
            //    Plugin.PatchLogger.LogInfo(saveSlotField.GetValue(GameManager.Instance));
            //} catch (Exception e) {
            //    Plugin.PatchLogger.LogInfo(e);
            //}

            // Connect to AP
            ArchipelagoConnection.TryDisconnect();

            Plugin.itemStorage = new ItemStorage(GameManager._platformMgr.ActiveSaverIndex);
            ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection("localhost:38281", "Droppel");

            ArchipelagoConnection.session.Items.ItemReceived += (receivedItemsHelper) => {
                var itemReceived = receivedItemsHelper.DequeueItem();
                Plugin.PatchLogger.LogInfo($"ItemReceived: {itemReceived.Item}");

                Plugin.itemStorage.RewardItem(itemReceived, receivedItemsHelper.Index);
            };

            //ArchipelagoConnection.session.Socket.PacketReceived += (packet) => {
            //    Logger.LogInfo($"Packet Received: {packet.ToJObject()}");
            //};

            archipelagoConnection.Connect();
            return;
        }
        [HarmonyPostfix]
        [HarmonyPatch("StartNewGameAt", new Type[] { typeof(int) })]
        private static void StartNewGameAtPost(GameManager __instance, int slotIndex) {
            ItemStorage.ResetFile(slotIndex);
            // Connect to AP
            ArchipelagoConnection.TryDisconnect();

            Plugin.itemStorage = new ItemStorage(slotIndex);
            ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection("localhost:38281", "Droppel");

            ArchipelagoConnection.session.Items.ItemReceived += (receivedItemsHelper) => {
                var itemReceived = receivedItemsHelper.DequeueItem();
                Plugin.PatchLogger.LogInfo($"ItemReceived: {itemReceived.Item}");

                Plugin.itemStorage.RewardItem(itemReceived, receivedItemsHelper.Index);
            };

            //ArchipelagoConnection.session.Socket.PacketReceived += (packet) => {
            //    Logger.LogInfo($"Packet Received: {packet.ToJObject()}");
            //};

            archipelagoConnection.Connect();
        }
    }

    //StartNewGameAt
}
