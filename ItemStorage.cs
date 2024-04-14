using Archipelago.MultiClient.Net.Models;
using Blingame.Haak;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HaakRandomizer {
    public class ItemStorage {

        private int lastIndex;

        private static string path = "randoStore";

        public int currentSlot;

        public Dictionary<long, int> itemCount;
        public Mutex itemCountMutex = new Mutex();

        public ItemStorage(int _currentSlot) {
            currentSlot = _currentSlot;
            itemCount = new Dictionary<long, int>();
            LoadFile();
        }

        public int GetLastIndex() {
            return lastIndex;
        }

        private void PrintItemStorage() {
            foreach (long key in itemCount.Keys) {
                Plugin.PatchLogger.LogInfo($"{key}: {itemCount[key]}");
            }
        }

        public void RewardItem(NetworkItem itemReceived, int index) {
            itemCountMutex.WaitOne();
            if (itemCount.ContainsKey(itemReceived.Item)) {
                itemCount[itemReceived.Item] += 1;
            } else {
                itemCount[itemReceived.Item] = 1;
            }

            if (index <= lastIndex) {
                return;
            }

            string itemReceivedName;
            try {
                itemReceivedName = ArchipelagoConnection.session.Items.GetItemName(itemReceived.Item);
            } catch (Exception e) {
                Plugin.PatchLogger.LogInfo($"Error: {e}");
                itemCountMutex.ReleaseMutex();
                return;
            }

            // ... Handle item receipt here
            Plugin.PatchLogger.LogInfo($"ItemReceivedName: {itemReceivedName}");
            lastIndex = index;

            SkillTree instance = PersistentSingleton<SkillTree>.Instance;
            switch (itemReceivedName) {
                case "KeyCard":
                    var keycard = new InventoryItem();
                    keycard.ItemID = "item_keycard0";
                    MMEventManager.TriggerEvent<MMInventoryEvent>(new MMInventoryEvent(MMInventoryEventType.Pick, "MainInventory", keycard, null));
                    break;
                case "Glove":
                    instance.Unlock(null, true, 1);
                    break;
                case "ProgressiveChargeAttack":
                    switch (itemCount[itemReceived.Item]) {
                        case 1:
                            instance.Unlock(null, true, 25); // 1: Charge Attack
                            break;
                        case 2:
                            instance.Unlock(null, true, 25); // 1: Charge Attack
                            instance.Unlock(null, true, 35); // 2: Immediate Charge
                            break;
                        case 3:
                            instance.Unlock(null, true, 25); // 1: Charge Attack
                            instance.Unlock(null, true, 35); // 2: Immediate Charge
                            instance.Unlock(null, true, 37); // 3: Boosted Charge
                            break;
                    }
                    break;
                case "ProgressiveSwordWaves":
                    UnlockMultiSkill(instance, itemReceived.Item, 36);
                    break;
                case "ProgressiveDash":
                    switch (itemCount[itemReceived.Item]) {
                        case 1:
                            instance.Unlock(null, true, 18); // 1: First Dash
                            break;
                        case 2:
                            instance.Unlock(null, true, 18); // 1: First Dash
                            instance.Unlock(null, true, 28); // 2: Unlimited slide
                            break;
                        case 3:
                            instance.Unlock(null, true, 18); // 1: First Dash
                            instance.Unlock(null, true, 28); // 2: Unlimited slide
                            instance.Unlock(null, true, 13); // 3: Full Directional Dash
                            break;
                        case 4:
                            instance.Unlock(null, true, 18); // 1: First Dash
                            instance.Unlock(null, true, 28); // 2: Unlimited slide
                            instance.Unlock(null, true, 13); // 3: Full Directional Dash
                            instance.Unlock(null, true, 12); // 3: Dash Damage
                            break;
                    }
                    break;
                case "Deflection":
                    instance.Unlock(null, true, 11);
                    break;
                case "ProgressiveHook":
                    switch (itemCount[itemReceived.Item]) {
                        case 1:
                            instance.Unlock(null, true, 40); // 1: Piercing Hook
                            break;
                        case 2:
                            instance.Unlock(null, true, 40); // 1: Piercing Hook
                            instance.Unlock(null, true, 38); // 2: Electric Hook
                            break;
                        case 3:
                            instance.Unlock(null, true, 40); // 1: Piercing Hook
                            instance.Unlock(null, true, 38); // 2: Electric Hook
                            instance.Unlock(null, true, 42); // 3: Ultimate Hook
                            break;
                    }
                    break;
                case "ProgressiveDivingThrust":
                    switch (itemCount[itemReceived.Item]) {
                        case 1:
                            instance.Unlock(null, true, 31); // 1: Diving Thrust
                            break;
                        case 2:
                            instance.Unlock(null, true, 44); // 2: Power Diving Thrust
                            break;
                    }
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
                case "Hacker":
                    instance.Unlock(null, true, 41);
                    break;
                case "LifeShard":
                    int currentHPFrags = PersistentSingleton<SkillTree>.Instance.HpFragments.CurrentCount;
                    PersistentSingleton<SkillTree>.Instance.HpFragments.AddFragment(itemCount[itemReceived.Item] - currentHPFrags);
                    break;
                case "EnergyShard":
                    int currentEPFrags = PersistentSingleton<SkillTree>.Instance.EpFragments.CurrentCount;
                    PersistentSingleton<SkillTree>.Instance.EpFragments.AddFragment(itemCount[itemReceived.Item] - currentEPFrags);
                    break;
                case "AttackBoost":
                    UnlockMultiSkill(instance, itemReceived.Item, 8);
                    break;
                case "Hookspeed":
                    UnlockMultiSkill(instance, itemReceived.Item, 9);
                    break;
                case "Hooklength":
                    UnlockMultiSkill(instance, itemReceived.Item, 3);
                    break;
                case "CritRate":
                    UnlockMultiSkill(instance, itemReceived.Item, 10);
                    break;
                case "FirstAid":
                    UnlockMultiSkill(instance, itemReceived.Item, 19);
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
                    CollectMultiItem(itemReceived.Item, 5);
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
                case "DyeWhite":
                    PersistentSingleton<ColleManager>.Instance.Collect(66, ColleUpdateType.Acquire);
                    break;
                case "Tape Red":
                    PersistentSingleton<ColleManager>.Instance.Collect(68, ColleUpdateType.Acquire);
                    break;
                case "Tape Blue":
                    PersistentSingleton<ColleManager>.Instance.Collect(67, ColleUpdateType.Acquire);
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
                case "Newspaper#9":
                    PersistentSingleton<ColleManager>.Instance.Collect(81, ColleUpdateType.Acquire);
                    break;
                case "Newspaper#10":
                    PersistentSingleton<ColleManager>.Instance.Collect(82, ColleUpdateType.Acquire);
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
                case "Developers log #10":
                    PersistentSingleton<ColleManager>.Instance.Collect(105, ColleUpdateType.Acquire);
                    break;
                case "Developers log #20":
                    PersistentSingleton<ColleManager>.Instance.Collect(149, ColleUpdateType.Acquire);
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
                case "Wasteland Reclaimer Poster":
                    PersistentSingleton<ColleManager>.Instance.Collect(94, ColleUpdateType.Acquire);
                    break;
                case "Hacking Device Manual":
                    PersistentSingleton<ColleManager>.Instance.Collect(118, ColleUpdateType.Acquire);
                    break;
                case "Daily Photo of Genie":
                    PersistentSingleton<ColleManager>.Instance.Collect(148, ColleUpdateType.Acquire);
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
                case "Letter of Commission":
                    PersistentSingleton<ColleManager>.Instance.Collect(117, ColleUpdateType.Acquire);
                    break;
                case "Report: Test Subject No. 36":
                    PersistentSingleton<ColleManager>.Instance.Collect(98, ColleUpdateType.Acquire);
                    break;
                case "Ms Lees Diary":
                    PersistentSingleton<ColleManager>.Instance.Collect(1, ColleUpdateType.Acquire);
                    break;
            }
            itemCountMutex.ReleaseMutex();
        }

        private void UnlockMultiSkill(SkillTree instance, long itemID, int skillID) {
            int current= instance.GetUnlockCount(skillID);
            Plugin.PatchLogger.LogInfo($"Current: {current}, APCount: {itemCount[itemID]}");
            for (int i = current; i < itemCount[itemID]; i++) {
                Plugin.PatchLogger.LogInfo($"Unlocked; i: {i}");
                instance.Unlock(null, true, skillID);
            }
        }

        private void CollectMultiItem(long itemID, int colleID) {
            int current = PersistentSingleton<ColleManager>.Instance.Colles[colleID].runtimeState.Progress;
            Plugin.PatchLogger.LogInfo($"Current: {current}, APCount: {itemCount[itemID]}");
            for (int i = current; i < itemCount[itemID]; i++) {
                PersistentSingleton<ColleManager>.Instance.Collect(colleID, ColleUpdateType.AcquireAndIncProgress);
            }
        }

        public void StoreFile() {
            File.WriteAllText(path + currentSlot, lastIndex.ToString());
        }

        public void LoadFile() {
            if (!File.Exists(path + currentSlot)) {
                lastIndex = 0;
                StoreFile();
                return;
            }
            string content = File.ReadAllText(path + currentSlot);

            lastIndex = int.Parse(content);
        }

        public static void ResetFile(int slot) {
            File.WriteAllText(path + slot, "0");
        }
    }
}
