using System;
using UnityEngine;

internal static class RangefinderSystem
{
    private static RangefinderHUD rangefinderHUD;
    private static bool isInitialized;

    private static bool EnableRangefinder =>
        RangefinderMod.enableRangefinder != null && RangefinderMod.enableRangefinder.Value;

    private static float MaxRange =>
        RangefinderMod.rangefinderMaxRange != null ? RangefinderMod.rangefinderMaxRange.Value : 500f;

    private static LayerMask raycastLayers;

    static RangefinderSystem()
    {
        raycastLayers = LayerMask.GetMask("Default", "Terrain", "Environment");
    }

    public static void Initialize()
    {
        if (isInitialized && rangefinderHUD != null && rangefinderHUD.IsAlive)
            return;

        try
        {
            if (rangefinderHUD == null)
                rangefinderHUD = new RangefinderHUD();

            rangefinderHUD.Setup();
            rangefinderHUD.SetEnabled(EnableRangefinder);
            isInitialized = rangefinderHUD.IsAlive;
        }
        catch (Exception ex)
        {
            SparrohPlugin.Logger.LogError($"Failed to initialize Rangefinder: {ex.Message}");
            isInitialized = false;
        }
    }

    public static void UpdateRangefinder()
    {
        if (!EnableRangefinder)
            return;

        // Recreate if HUD was destroyed (menu / mission teardown)
        if (rangefinderHUD == null || !rangefinderHUD.IsAlive)
        {
            isInitialized = false;
            Initialize();
            if (!isInitialized)
                return;
        }

        try
        {
            if (Camera.main == null)
            {
                rangefinderHUD.UpdateRange(-1f);
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, MaxRange, raycastLayers))
                rangefinderHUD.UpdateRange(hit.distance);
            else
                rangefinderHUD.UpdateRange(-1f);
        }
        catch (Exception ex)
        {
            SparrohPlugin.Logger.LogError($"Error updating rangefinder: {ex.Message}");
            rangefinderHUD?.UpdateRange(-1f);
        }
    }

    public static void SetEnabled(bool enabled)
    {
        if (rangefinderHUD != null)
            rangefinderHUD.SetEnabled(enabled);
    }

    public static void Cleanup()
    {
        if (rangefinderHUD != null)
        {
            rangefinderHUD.Destroy();
            rangefinderHUD = null;
        }
        isInitialized = false;
    }
}
