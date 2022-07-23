using HarmonyLib;
using UnityEngine;

namespace EasyMod.patches
{
    [HarmonyPatch(typeof(PlayerCharacterStats), nameof(PlayerCharacterStats.OnUpdateStats))]
    class PlayerCharacterStats_OnUpdateStats
    {
        static bool Prefix(PlayerCharacterStats __instance)
        {

            Character character = __instance.m_character;

            if (!character.IsDead)
            {
                if (ModSettings.HealthRegen.IsEnabled())
                {
                    __instance.m_healthRegen = new Stat(ModSettings.HealthRegen.Value());
                }

                if (ModSettings.StaminaRegen.IsEnabled())
                {
                    __instance.m_staminaRegen = new Stat(ModSettings.StaminaRegen.Value());
                }

                if (ModSettings.ManaRegen.IsEnabled())
                {
                    __instance.m_manaRegen = new Stat(ModSettings.ManaRegen.Value());
                }

                if (ModSettings.StabilityRegen.IsEnabled())
                {
                    __instance.m_stabilityRegen = new Stat(ModSettings.StabilityRegen.Value());
                }

                float updateDeltaTime = (__instance.m_lastUpdateTime == -999f) ? 0f : (Time.time - __instance.m_lastUpdateTime);

                if (ModSettings.HealthRecover.IsEnabled())
                {
                    __instance.m_health = ModUtil.CalculateValue(ModSettings.HealthRecover.Value(), __instance.m_health, __instance.ActiveMaxHealth, updateDeltaTime);
                }

                if (ModSettings.StaminaRecover.IsEnabled())
                {
                    __instance.m_stamina = ModUtil.CalculateValue(ModSettings.StaminaRecover.Value(), __instance.m_stamina, __instance.ActiveMaxStamina, updateDeltaTime);
                }

                if (ModSettings.ManaRecover.IsEnabled())
                {
                    __instance.m_mana = ModUtil.CalculateValue(ModSettings.ManaRecover.Value(), __instance.m_mana, __instance.ActiveMaxMana, updateDeltaTime);
                }

                if (ModSettings.BurntHealthRecover.IsEnabled())
                {
                    __instance.m_burntHealth = ModUtil.CalculateValue(-ModSettings.BurntHealthRecover.Value(), __instance.m_burntHealth, __instance.MaxHealth, updateDeltaTime);
                }

                if (ModSettings.BurntStaminaRecover.IsEnabled())
                {
                    __instance.m_burntStamina = ModUtil.CalculateValue(-ModSettings.BurntStaminaRecover.Value(), __instance.m_burntStamina, __instance.MaxStamina, updateDeltaTime);
                }

                if (ModSettings.BurntManaRecover.IsEnabled())
                {
                    __instance.m_burntMana = ModUtil.CalculateValue(-ModSettings.BurntManaRecover.Value(), __instance.m_burntMana, __instance.MaxMana, updateDeltaTime);
                }

                if (ModSettings.CorruptionRecover.IsEnabled() && __instance.m_corruptionLevel > 0)
                {
                    __instance.m_corruptionLevel = ModUtil.CalculateValue(-ModSettings.CorruptionRecover.Value(), __instance.m_corruptionLevel, __instance.m_corruptionLevel, updateDeltaTime);
                }
            }

            return true;
        }

    }
}
