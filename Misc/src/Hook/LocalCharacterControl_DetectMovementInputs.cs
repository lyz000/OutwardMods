using HarmonyLib;

namespace Misc.Hook
{
    [HarmonyPatch(typeof(LocalCharacterControl), nameof(LocalCharacterControl.DetectMovementInputs))]
    class LocalCharacterControl_DetectMovementInputs
    {
        static bool Prefix(LocalCharacterControl __instance)
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
