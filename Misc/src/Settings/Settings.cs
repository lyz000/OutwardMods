using BepInEx.Configuration;
using System;
using SideLoader;

namespace Misc
{
    class Settings
    {
        private static int Order = 0;
        private static ConfigFile Config;

        public const string SectionFunction = "Function";
        public static ConfigEntry<bool> EnableToggleSprintKey;

        public const string SectionItem = "Item";
        public static ConfigEntry<bool> DisplaySellPrice;
        public static ConfigEntry<bool> DisplayDurability;
        public static ConfigEntry<bool> SellItem;
        public static ConfigEntry<bool> RepairEquipment;
        public static ConfigEntry<bool> EnableOpenStashKey;

        public static CharacterShareData CharaData0;
        public static CharacterShareData CharaData1;
        public static CharacterShareData[] CharaDatas;

        public static class CustomKeyName
        {
            public const string ToggleSprint = "Toggle Sprint";
            public const string Forward = "Forward";
            public const string Backward = "Backward";
            public const string Left = "Left";
            public const string Right = "Right";

            public const string OpenStash1 = "Open Stash 1P";
            public const string OpenStash2 = "Open Stash 2P";
        }

        public static void Init(ConfigFile config)
        {
            Config = config;

            EnableToggleSprintKey = Bind(SectionFunction, nameof(EnableToggleSprintKey), true, "Enable/Disable Sprint key added in game input setting.");
            CustomKeybindings.AddAction(CustomKeyName.ToggleSprint, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Forward, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Backward, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Left, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Right, KeybindingsCategory.CustomKeybindings, ControlType.Both);

            DisplaySellPrice = Bind(SectionItem, nameof(DisplaySellPrice), true, "Display the estimated sell price on item detail.");
            DisplayDurability = Bind(SectionItem, nameof(DisplayDurability), true, "Display the durability for perishables on item detail.");
            SellItem = Bind(SectionItem, nameof(SellItem), true, "Add a sell action in item menu.");
            RepairEquipment = Bind(SectionItem, nameof(RepairEquipment), true, "Add a repair action for equipments in item menu, costs 10 coins");
            EnableOpenStashKey = Bind(SectionItem, nameof(EnableOpenStashKey), true, "Enable/Disable Open stash key added in game input setting.");
            CustomKeybindings.AddAction(CustomKeyName.OpenStash1, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.OpenStash2, KeybindingsCategory.CustomKeybindings, ControlType.Both);

            CharaData0 = new CharacterShareData(0);
            CharaData1 = new CharacterShareData(1);
            CharaDatas = new CharacterShareData[] { CharaData0, CharaData1 };
        }

        public static CharacterShareData GetCharacterShareData(int playerID)
        {
            if (playerID == 0)
            {
                return CharaData0;
            }
            else if (CharaData1.playerID == 1)
            {
                return CharaData1;
            }
            else
            {
                return null;
            }
        }

        public static void UseCharacterShareData(int playerID, Func<CharacterShareData, object> action)
        {
            var charaData = GetCharacterShareData(playerID);
            if (charaData == null || charaData.playerID < 0 || charaData.playerID != playerID)
            {
                return;
            }
            action(charaData);
        }

        public static void ForeachCharacterShareData(Func<CharacterShareData, object> action)
        {
            foreach (CharacterShareData charaData in CharaDatas)
            {
                action(charaData);
            }
        }

        private static ConfigEntry<T> Bind<T>(string section, string name, T value, string description)
        {
            Order = Order - 1;
            return Config.Bind(section, name, value, new ConfigDescription(description, null, new ConfigurationManagerAttributes { Order = Order }));
        }
    }
}
