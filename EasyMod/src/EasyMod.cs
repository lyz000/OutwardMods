using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace EasyMod
{
    [BepInPlugin(ModID, ModName, ModVersion)]
    public class EasyMod : BaseUnityPlugin
    {
        public const string ModID = "gunnuc.EasyMod";
        public const string ModName = "EasyMod";
        public const string ModVersion = "1.0.0";

        internal void Awake()
        {
            ModUtil.Log = this.Logger;
            ModUtil.Log.LogInfo($"{ModName} {ModVersion} awaken.");

            ModSettings.Init(Config);

            new Harmony(ModID).PatchAll();
        }
    }
}
