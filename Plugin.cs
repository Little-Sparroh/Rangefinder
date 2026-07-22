using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency("sparroh.uilibrary")]
[MycoMod(null, ModFlags.IsClientSide)]
public class SparrohPlugin : BaseUnityPlugin

{
    public const string PluginGUID = "sparroh.rangefinder";
    public const string PluginName = "Rangefinder";
    public const string PluginVersion = "1.0.1";

    internal static new ManualLogSource Logger;

    private Harmony harmony;
    private RangefinderMod rangefinder;

    private void Awake()
    {
        Logger = base.Logger;

        try
        {
            harmony = new Harmony(PluginGUID);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to create Harmony instance: {ex.Message}");
            return;
        }

        var configFile = Config;
        try
        {
            var watcher = new FileSystemWatcher(Paths.ConfigPath, "sparroh.rangefinder.cfg");
            watcher.Changed += (s, e) =>
            {
                configFile.Reload();
            };
            watcher.EnableRaisingEvents = true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"Failed to set up config watcher: {ex.Message}");
        }

        try
        {
            rangefinder = new RangefinderMod(configFile, harmony);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to initialize Rangefinder: {ex.Message}");
        }

        try
        {
            harmony.PatchAll();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to apply Harmony patches: {ex.Message}");
        }

        Logger.LogInfo($"{PluginName} loaded successfully.");
    }

    private void Update()
    {
        try
        {
            if (rangefinder != null) rangefinder.UpdateHudVisibility();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error in Rangefinder.UpdateHudVisibility(): {ex.Message}");
        }

        try
        {
            if (rangefinder != null) rangefinder.Update();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error in Rangefinder.Update(): {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        try
        {
            if (rangefinder != null) rangefinder.OnDestroy();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error in Rangefinder.OnDestroy(): {ex.Message}");
        }

        try
        {
            if (harmony != null) harmony.UnpatchSelf();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error unpatching Harmony: {ex.Message}");
        }
    }
}
