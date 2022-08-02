using BepInEx;
using HarmonyLib;
using SideLoader;

namespace Misc
{
    [BepInPlugin(ModID, ModName, ModVersion)]
    public class Misc : BaseUnityPlugin
    {
        public const string ModID = "gunnuc.outward.misc";
        public const string ModName = "Misc";
        public const string ModVersion = "1.0.0";

        internal void Awake()
        {
            ModUtil.Logger = Logger;
            ModUtil.Logger.LogInfo($"{ModName} {ModVersion} awaken.");

            Settings.Init(Config);

            new Harmony(ModID).PatchAll();
        }

        internal void Update()
        {
            if (Settings.EnableToggleSprintKey.Value)
            {
                // Handle KeyPress for toggle sprint
                if (CustomKeybindings.GetKeyDown(Settings.CustomKeyName.ToggleSprint, out int playerIDToggleSprint))
                {
                    Settings.UseCharacterShareData(playerIDToggleSprint, charaData =>
                    {
                        charaData.sprint = !charaData.sprint;
                        return 0;
                    });
                }

                // Handle KeyPress for toggle sprint directions
                var keyForwardHeld = CustomKeybindings.GetKey(Settings.CustomKeyName.Forward);
                var keyBackwardHeld = CustomKeybindings.GetKey(Settings.CustomKeyName.Backward);
                var keyLeftHeld = CustomKeybindings.GetKey(Settings.CustomKeyName.Left);
                var keyRightHeld = CustomKeybindings.GetKey(Settings.CustomKeyName.Right);
                // TODO fix which player Helds
                if (!keyForwardHeld && !keyBackwardHeld && !keyLeftHeld && !keyRightHeld)
                {
                    Settings.CharaData0.sprint = false;
                }
            }

            if (Settings.EnableOpenStashKey.Value)
            {
                // Handle KeyPress for open stash 1P
                if (CustomKeybindings.GetKeyDown(Settings.CustomKeyName.OpenStash1, out int playerIDOpenStash1))
                {
                    ModUtil.ShowStashPanel(playerIDOpenStash1, Settings.CharaData0.playerID);
                }
                // Handle KeyPress for open stash 2P
                if (CustomKeybindings.GetKeyDown(Settings.CustomKeyName.OpenStash2, out int playerIDOpenStash2))
                {
                    ModUtil.ShowStashPanel(playerIDOpenStash1, Settings.CharaData1.playerID);
                }
            }
        }
    }
}
