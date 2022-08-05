using HarmonyLib;
using System;

namespace Misc.Patch
{
    [HarmonyPatch(typeof(Character))]
    class Character_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(MethodType.Getter)]
        [HarmonyPatch(nameof(Character.IsSuperSpeedActive))]
        static bool IsSuperSpeedActive_Getter_Prefix(Character __instance, ref bool __result)
        {
            if (!Settings.EnableSuperSpeedKey.Value)
            { 
                return true;
            }

            __result = false;
            var character = __instance;
            if (character.Name == "Code Sonic")
            { 
                return true;
            }

            __result = Settings.GetCharacterShareData(character.OwnerPlayerSys.PlayerID).superSpeed;
            return false;
        }
    }
}
