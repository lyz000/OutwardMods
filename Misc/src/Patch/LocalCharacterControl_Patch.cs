using HarmonyLib;

namespace Misc.Patch
{
    [HarmonyPatch(typeof(LocalCharacterControl))]
    class LocalCharacterControl_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(LocalCharacterControl.DetectMovementInputs))]
        static bool DetectMovementInputs_Prefix(LocalCharacterControl __instance)
        {
            if (!Settings.EnableToggleSprintKey.Value)
            {
                return true;
            }

            var localCharacterControl = __instance;
            var playerID = localCharacterControl.Character.OwnerPlayerSys.PlayerID;
            // if character move distance is zero, stop sprint
            if (ControlsInput.MoveHorizontal(playerID) == 0f && ControlsInput.MoveVertical(playerID) == 0f)
            {
                Settings.GetCharacterShareData(playerID).sprint = false;
            }

            return true;
        }
    }
}
