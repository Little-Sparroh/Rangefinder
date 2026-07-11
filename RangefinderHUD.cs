using Sparroh.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// Thin wrapper around HudHandle for the rangefinder display.
/// </summary>
public class RangeFinderHUD
{
    private HudHandle hud;

    public bool IsAlive => hud != null && hud.GameObject != null && hud.Primary != null;

    public void Setup()
    {
        if (IsAlive)
            return;

        // Prefer reticle (consistent with other Sparroh HUDs); fall back to DefaultHUDParent.
        Transform parent = null;
        if (UIHelpers.TryGetReticle(out var reticle))
            parent = reticle;
        else if (Pigeon.Movement.PlayerLook.Instance != null)
            parent = Pigeon.Movement.PlayerLook.Instance.DefaultHUDParent;

        var builder = HudBuilder.Create("RangeFinderHUD")
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

        if (distance >= 1000f)
            hud.Primary.Text = RichText.Colorize("∞ m", UIColors.Sky);
        else if (distance < 0f)
            hud.Primary.Text = RichText.Colorize("--- m", UIColors.TextMuted);
        else
            hud.Primary.Text = RichText.Colorize($"{distance:F1} m", UIColors.Sky);
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
            if (hud.GameObject != null)
                hud.Destroy();
            hud = null;
        }
    }
}
