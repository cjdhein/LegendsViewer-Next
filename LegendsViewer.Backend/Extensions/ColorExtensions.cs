using System.Drawing;

namespace LegendsViewer.Backend.Extensions;

public static class ColorExtensions
{
    public static string ToRgbaString(this Color color, float alpha = 1.0f)
    {
        alpha = Math.Clamp(alpha, 0f, 1f);

        return $"rgba({color.R}, {color.G}, {color.B}, {alpha.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
    }
}
