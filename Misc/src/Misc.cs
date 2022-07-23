using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Misc
{
    [BepInPlugin(ModID, ModName, ModVersion)]
    public class Misc : BaseUnityPlugin
    {
        public const string ModID = "gunnuc.outward.misc";
        public const string ModName = "Misc";
        public const string ModVersion = "1.0.0";

        CharacterShareData character1 = new CharacterShareData();
        CharacterShareData character2 = new CharacterShareData();
        

        public static class ShareData {
            private static int pID = -1;
            public static bool Sprint = false;
        }

        internal void Awake()
        {
            Logger.LogInfo($"{ModName} {ModVersion} awaken.");

            Settings.Init(Config);

            new Harmony(ModID).PatchAll();
        }

        internal void Update()
        {
            // Handle KeyPress
            if (CustomKeybindings.GetKeyDown(Settings.ToggleSprint, out int playerID))
            {
                var cha = playerID == 1 ? character1 : character2;
                cha.PlayerID = playerID;
                cha.Sprint = !cha.Sprint;
            }
        }
    }
}
