using System;
using HarmonyLib;

namespace Misc.Patch
{
    [HarmonyPatch(typeof(ControlsInput))]
    class ControlsInput_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ControlsInput.Sprint))]
        [HarmonyPatch(new Type[] { typeof(int) })]
        static bool Sprint_Prefix(ControlsInput __instance, int _playerID, ref bool __result)
        {
            if (Settings.EnableToggleSprintKey.Value)
            {
                bool sprint = false;
                Settings.UseCharacterShareData(_playerID, charaData =>
                {
                    // replace ControlsInput.Sprint's return value
                    sprint = charaData.sprint;
                    return 0;
                });
                __result = sprint;
                return false;
            }
            return true;
        }
    }
}
