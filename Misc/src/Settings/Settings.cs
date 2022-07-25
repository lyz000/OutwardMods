using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SideLoader;

namespace Misc
{
    class Settings
    {
        private static ConfigFile Config;
        private static int Order = 0;

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
        }

        public static void Init(ConfigFile config)
        {
            Config = config;
            CharaData0 = new CharacterShareData(0);
            CharaData1 = new CharacterShareData(1);
            CharaDatas = new CharacterShareData[] { CharaData0, CharaData1 };

            CustomKeybindings.AddAction(CustomKeyName.ToggleSprint, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Forward, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Backward, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Left, KeybindingsCategory.CustomKeybindings, ControlType.Both);
            CustomKeybindings.AddAction(CustomKeyName.Right, KeybindingsCategory.CustomKeybindings, ControlType.Both);
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
