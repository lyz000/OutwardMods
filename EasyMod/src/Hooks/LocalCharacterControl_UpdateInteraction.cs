using HarmonyLib;

namespace EasyMod.patches
{
    [HarmonyPatch(typeof(LocalCharacterControl), nameof(LocalCharacterControl.UpdateInteraction))]
    class LocalCharacterControl_UpdateInteraction
    {
        static void Postfix(LocalCharacterControl __instance)
        {
            if (__instance.InputLocked || __instance.Character.CharacterUI.ChatPanel.IsChatFocused)
            {
                return;
            }

            if (ModSettings.ResetBreakThrough.Value().IsDown())
            {
                __instance.Character.PlayerStats.m_usedBreakthroughCount = 0;
            }

        }

    }
}
