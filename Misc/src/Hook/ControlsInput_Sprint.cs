using HarmonyLib;
using System;

namespace Misc.Hook
{
    [HarmonyPatch(typeof(ControlsInput), nameof(ControlsInput.Sprint), new Type[] { typeof(int) })]
    class ControlsInput_Sprint
    {
        static void Postfix(ControlsInput __instance, int _playerID, ref bool __result)
        {
            bool sprint = false;
            Settings.UseCharacterShareData(_playerID, (charaData) => 
            {
                // replace ControlsInput.Sprint return value
                sprint = charaData.sprint;
                return 0;
            });
            __result = sprint;
        }
    }
}
