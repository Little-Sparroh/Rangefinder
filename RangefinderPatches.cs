using BepInEx.Configuration;
using HarmonyLib;
using Sparroh.UI;
using System;


public class RangefinderMod
{
    public static ConfigEntry<bool> enableRangefinder;
    public static ConfigEntry<float> rangefinderMaxRange;
    public static ConfigColor valueColor;
    public static ConfigColor noTargetColor;
    private readonly ConfigFile configFile;
    private readonly Harmony harmony;

    public RangefinderMod(ConfigFile configFile, Harmony harmony)
    {
        this.configFile = configFile;
        this.harmony = harmony;

        enableRangefinder = configFile.Bind("General", "EnableRangefinderHUD", true, "Enable the rangefinder display");
        rangefinderMaxRange = configFile.Bind("General", "RangefinderMaxRange", 500f, "Maximum range for rangefinder (meters)");

        valueColor = ConfigColor.Bind(configFile, "Colors", "ValueColor", UIColors.Sky,
            "Rich-text color for range values (hex RRGGBB or #RRGGBB).");
        noTargetColor = ConfigColor.Bind(configFile, "Colors", "NoTargetColor", UIColors.TextMuted,
            "Rich-text color when no target is hit (hex RRGGBB or #RRGGBB).");

        enableRangefinder.SettingChanged += OnEnableRangefinderChanged;

        harmony.PatchAll(typeof(RangefinderPatches));
    }


    public void UpdateHudVisibility()
    {
        RangefinderSystem.SetEnabled(enableRangefinder.Value);
    }

    public void Update()
    {
        RangefinderSystem.UpdateRangefinder();
    }

    public void OnDestroy()
    {
        RangefinderSystem.Cleanup();
        harmony.UnpatchSelf();
    }

    private void OnEnableRangefinderChanged(object sender, EventArgs e)
    {
        UpdateHudVisibility();
    }

    [HarmonyPatch]
    public static class RangefinderPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MissionHUD), "Start")]
        public static void MissionHUD_Start_Postfix()
        {
            RangefinderSystem.Initialize();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MissionHUD), "Update")]
        public static void MissionHUD_Update_Postfix()
        {
            RangefinderSystem.UpdateRangefinder();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MissionHUD), "OnDestroy")]
        public static void MissionHUD_OnDestroy_Postfix()
        {
            RangefinderSystem.Cleanup();
        }
    }
}
