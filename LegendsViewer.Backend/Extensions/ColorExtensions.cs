using SkiaSharp;
using System.Drawing;

namespace LegendsViewer.Backend.Extensions;

public static class ColorExtensions
{
    public static string ToRgbaString(this Color color, float alpha = 1.0f)
    {
        alpha = Math.Clamp(alpha, 0f, 1f);

        return $"rgba({color.R}, {color.G}, {color.B}, {alpha.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
    }

    public static string ToRgbaString(this SKColor color, float alpha = 1.0f)
    {
        alpha = Math.Clamp(alpha, 0f, 1f);

        return $"rgba({color.Red}, {color.Green}, {color.Blue}, {alpha.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
    }
}
