using HarmonyLib;
using System;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(ControlsInput))]
    class ControlsInput_Patcher
    {
        [HarmonyPatch(nameof(ControlsInput.Sprint), new Type[] { typeof(int) }), HarmonyPostfix]
        static void Sprint_Postfix(ControlsInput __instance, int _playerID, ref bool __result)
        {
            bool sprint = false;
            Settings.UseCharacterShareData(_playerID, (charaData) => 
            {
                // replace ControlsInput.Sprint's return value
                sprint = charaData.sprint;
                return 0;
            });
            __result = sprint;
        }
    }
}
