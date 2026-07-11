using System;
using UnityEngine;

internal static class RangeFinderSystem
{
    private static RangeFinderHUD rangeFinderHUD;
    private static bool isInitialized;

    private static bool EnableRangeFinder =>
        RangeFinderMod.enableRangeFinder != null && RangeFinderMod.enableRangeFinder.Value;

    private static float MaxRange =>
        RangeFinderMod.rangeFinderMaxRange != null ? RangeFinderMod.rangeFinderMaxRange.Value : 500f;

    private static LayerMask raycastLayers;

    static RangeFinderSystem()
    {
        raycastLayers = LayerMask.GetMask("Default", "Terrain", "Environment");
    }

    public static void Initialize()
    {
        if (isInitialized && rangeFinderHUD != null && rangeFinderHUD.IsAlive)
            return;

        try
        {
            if (rangeFinderHUD == null)
                rangeFinderHUD = new RangeFinderHUD();

            rangeFinderHUD.Setup();
            rangeFinderHUD.SetEnabled(EnableRangeFinder);
            isInitialized = rangeFinderHUD.IsAlive;
        }
        catch (Exception ex)
        {
            SparrohPlugin.Logger.LogError($"Failed to initialize RangeFinder: {ex.Message}");
            isInitialized = false;
        }
    }

    public static void UpdateRangeFinder()
    {
        if (!EnableRangeFinder)
            return;

        // Recreate if HUD was destroyed (menu / mission teardown)
        if (rangeFinderHUD == null || !rangeFinderHUD.IsAlive)
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
                rangeFinderHUD.UpdateRange(-1f);
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, MaxRange, raycastLayers))
                rangeFinderHUD.UpdateRange(hit.distance);
            else
                rangeFinderHUD.UpdateRange(-1f);
        }
        catch (Exception ex)
        {
            SparrohPlugin.Logger.LogError($"Error updating rangefinder: {ex.Message}");
            rangeFinderHUD?.UpdateRange(-1f);
        }
    }

    public static void SetEnabled(bool enabled)
    {
        if (rangeFinderHUD != null)
            rangeFinderHUD.SetEnabled(enabled);
    }

    public static void Cleanup()
    {
        if (rangeFinderHUD != null)
        {
            rangeFinderHUD.Destroy();
            rangeFinderHUD = null;
        }
        isInitialized = false;
    }
}
