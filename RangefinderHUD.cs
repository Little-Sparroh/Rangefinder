using Sparroh.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// Thin wrapper around HudHandle for the rangefinder display.
/// </summary>
public class RangefinderHUD
{
    private HudHandle hud;

    public bool IsAlive => HudHandle.IsValid(hud) && hud.Primary != null;

    public void Setup()
    {
        if (hud != null && !hud.IsAlive)
            hud = null;

        if (IsAlive)
            return;


        // Prefer reticle (consistent with other Sparroh HUDs); fall back to DefaultHUDParent.
        Transform parent = null;
        if (UIHelpers.TryGetReticle(out var reticle))
            parent = reticle;
        else if (Pigeon.Movement.PlayerLook.Instance != null)
            parent = Pigeon.Movement.PlayerLook.Instance.DefaultHUDParent;

        var builder = HudBuilder.Create("RangefinderHUD")
            .Size(200f, 40f)
            .Pivot(new Vector2(0.5f, 0.5f))
            .AddText("RangeText", fontSize: 22f, alignment: TextAlignmentOptions.Center);

        if (parent != null)
            builder.Parent(parent);
        else
            builder.ParentToReticle();

        // Center of screen-ish when on reticle
        builder.Anchor(0.5f, 0.42f);

        hud = builder.Build();
    }

    public void UpdateRange(float distance)
    {
        if (!IsAlive || hud.Primary == null)
            return;

        Color value = RangefinderMod.valueColor != null ? RangefinderMod.valueColor.Value : UIColors.Sky;
        Color noTarget = RangefinderMod.noTargetColor != null ? RangefinderMod.noTargetColor.Value : UIColors.TextMuted;

        if (distance >= 1000f)
            hud.Primary.Text = RichText.Colorize("∞ m", value);
        else if (distance < 0f)
            hud.Primary.Text = RichText.Colorize("--- m", noTarget);
        else
            hud.Primary.Text = RichText.Colorize($"{distance:F1} m", value);

    }

    public void SetEnabled(bool enabled)
    {
        if (IsAlive)
            hud.SetActive(enabled);
    }

    public void Destroy()
    {
        if (hud != null)
        {
            if (hud.IsAlive)
                hud.Destroy();
            hud = null;
        }
    }

}
