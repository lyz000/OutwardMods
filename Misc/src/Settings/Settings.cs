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

        public const string ToggleSprint = "Toggle Sprint";


        public static void Init(ConfigFile config)
        {
            CustomKeybindings.AddAction(ToggleSprint, KeybindingsCategory.CustomKeybindings, ControlType.Both);
        }

        private static ConfigEntry<T> Bind<T>(string section, string name, T value, string description)
        {
            Order = Order - 1;
            return Config.Bind(section, name, value, new ConfigDescription(description, null, new ConfigurationManagerAttributes { Order = Order }));
        }
    }
}
