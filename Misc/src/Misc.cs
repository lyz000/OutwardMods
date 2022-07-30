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
            // Handle KeyPress
            if (CustomKeybindings.GetKeyDown(Settings.CustomKeyName.ToggleSprint, out int playerID))
            {
                Settings.UseCharacterShareData(playerID, charaData =>
                {
                    charaData.sprint = !charaData.sprint;
                    return 0;
                });
            }

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
    }
}
