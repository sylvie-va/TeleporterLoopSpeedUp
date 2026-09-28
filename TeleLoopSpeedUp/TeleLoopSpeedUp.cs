using RoR2;
using BepInEx;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using R2API.Utils;

namespace TeleLoopSpeedUp
{
    [BepInPlugin("sylvie.TeleLoopSpeedUp", "TeleLoopSpeedUp", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public sealed class TeleLoopSpeedUp : BaseUnityPlugin
    {
        private static float perLoopMultiplier = 2f;
            void Awake()
            {
                var config = Config.Bind("Teleporter Speed Up Per Loop", 
                                                            "Multiplier", 
                                                            perLoopMultiplier, 
                                                            "Exponentional Teleporter speedup multiplier.");

                // Slider: Teleporter Speedup Multiplier
                ModSettingsManager.AddOption(new StepSliderOption(config, new StepSliderConfig{ min = 1f, max = 5f, increment = 0.1f , FormatString = "{0:0.00}" }));

                On.RoR2.HoldoutZoneController.OnEnable += HoldoutZoneController_OnEnable;
            }
            private static void HoldoutZoneController_OnEnable(On.RoR2.HoldoutZoneController.orig_OnEnable orig, HoldoutZoneController self)
            {
                if (!Run.instance)
                {
                    orig(self);
                    return;
                }
                self.baseChargeDuration /= UnityEngine.Mathf.Pow(perLoopMultiplier, Run.instance.loopClearCount);
                orig(self);
            }
    }
}
