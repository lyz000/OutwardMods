using BepInEx.Configuration;
using UnityEngine;

namespace EasyMod
{
    class ModSettings
    {
        public const string CatagoryRegen = "01. StatRegen";
        public static ConfigSet<float> HealthRegen;
        public static ConfigSet<float> StaminaRegen;
        public static ConfigSet<float> ManaRegen;
        public static ConfigSet<float> StabilityRegen;

        public const string CatagoryStatRecover = "02. StatRecvoer";
        public static ConfigSet<float> HealthRecover;
        public static ConfigSet<float> StaminaRecover;
        public static ConfigSet<float> ManaRecover;
        public static ConfigSet<float> BurntHealthRecover;
        public static ConfigSet<float> BurntStaminaRecover;
        public static ConfigSet<float> BurntManaRecover;
        public static ConfigSet<float> CorruptionRecover;
        public static ConfigSet<float> RecoverOnEat;

        public const string CatagoryMisc = "03. Misc";
        public static ConfigSet<KeyboardShortcut> ResetBreakThrough;

        public static void Init(ConfigFile config)
        {
            var conf = new Conf(config);

            HealthRegen = conf.BindSet(CatagoryRegen, nameof(HealthRegen), 1.0f);
            StaminaRegen = conf.BindSet(CatagoryRegen, nameof(StaminaRegen), 10.0f);
            ManaRegen = conf.BindSet(CatagoryRegen, nameof(ManaRegen), 1.0f);
            StabilityRegen = conf.BindSet(CatagoryRegen, nameof(StabilityRegen), 10.0f);

            BurntHealthRecover = conf.BindSet(CatagoryStatRecover, nameof(BurntHealthRecover), 0.1f);
            BurntStaminaRecover = conf.BindSet(CatagoryStatRecover, nameof(BurntStaminaRecover), 0.1f);
            BurntManaRecover = conf.BindSet(CatagoryStatRecover, nameof(BurntManaRecover), 0.1f);
            HealthRecover = conf.BindSet(CatagoryStatRecover, nameof(HealthRecover), 1.0f);
            StaminaRecover = conf.BindSet(CatagoryStatRecover, nameof(StaminaRecover), 1.0f);
            ManaRecover = conf.BindSet(CatagoryStatRecover, nameof(ManaRecover), 1.0f);
            CorruptionRecover = conf.BindSet(CatagoryStatRecover, nameof(CorruptionRecover), 0.1f, " Corruption recover speed, should between 0 and 1000.");
            RecoverOnEat = conf.BindSet(CatagoryStatRecover, nameof(RecoverOnEat), 0.6f, " Eating, Drinking recovers HP in percent of ActiveMaxHealth, should between 0 and 1.");
            
            ResetBreakThrough = conf.BindSet(CatagoryMisc, nameof(ResetBreakThrough), new KeyboardShortcut(KeyCode.Backspace), " Reset used breakthrough count. Default: X");

        }

    }
}
