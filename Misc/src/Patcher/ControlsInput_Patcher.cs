using System;
using HarmonyLib;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(ControlsInput))]
    class ControlsInput_Patcher
    {
        [HarmonyPatch(nameof(ControlsInput.Sprint), new Type[] { typeof(int) }), HarmonyPrefix]
        static bool Sprint_Prefix(ControlsInput __instance, int _playerID, ref bool __result)
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
    }
}
