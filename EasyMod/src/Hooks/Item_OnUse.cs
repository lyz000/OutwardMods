using HarmonyLib;

namespace EasyMod.patches
{
    [HarmonyPatch(typeof(Item), nameof(Item.OnUse))]
    class Item_OnUse
    {
        static void Postfix(Item __instance, Character _targetChar)
        {
            if (ModSettings.RecoverOnEat.IsEnabled() && _targetChar.Stats.m_health < _targetChar.Stats.ActiveMaxHealth && (__instance.IsFood || __instance.IsDrink))
            {
                _targetChar.Stats.m_health = ModUtil.CalculateValue(_targetChar.Stats.ActiveMaxHealth * ModSettings.RecoverOnEat.Value(), _targetChar.Stats.m_health, _targetChar.Stats.ActiveMaxHealth, 1.0f);
            }
        }
    }
}
