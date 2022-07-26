using HarmonyLib;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(LocalCharacterControl))]
    class LocalCharacterControl_Patcher
    {
        [HarmonyPatch(nameof(LocalCharacterControl.DetectMovementInputs)), HarmonyPrefix]
        static bool DetectMovementInputs_Prefix(LocalCharacterControl __instance)
        {
            var localCharacterControl = __instance;
            var character = localCharacterControl.Character;
            Settings.UseCharacterShareData(character.OwnerPlayerSys.PlayerID, (charaData) => 
            {
                // if character move distance is zero, stop sprint
                if (ControlsInput.MoveHorizontal(charaData.playerID) == 0f && ControlsInput.MoveVertical(charaData.playerID) == 0f)
                {
                    charaData.sprint = false;
                }
                return 0;
            });
            return true;
        }
    }
}
