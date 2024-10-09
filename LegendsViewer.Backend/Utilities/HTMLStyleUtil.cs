using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace LegendsViewer.Backend.Utilities;

public static class HtmlStyleUtil
{
    public const string SymbolPopulation = "<span class=\"legends_symbol_population\">&#9823;</span>";
    public const string SymbolSite = "<span class=\"legends_symbol_site\">&#9978;</span>";
    public const string SymbolDead = "<span class=\"legends_symbol_dead\">&#10013;</span>";

    public static string CurrentDwarfObject(string name)
    {
        return $"<span class=\"legends_current_dwarfobject\">{name}</span>";
    }

    public static string GetIconString(string icon, string? color = null)
    {
        string colorString = "";
        if (color != null)
        {
            colorString = $"style=\"color:{color}\"";
        }
        return $"<i {colorString} class=\"mdi-{icon} mdi v-icon notranslate v-theme--dark v-icon--size-default\" aria-hidden=\"true\"></i>";
    }

    public static string GetCivIconString(string text, string color)
    {
        return $"<span style=\"background-color:{color}\" class=\"v-chip v-chip--label v-theme--dark v-chip--density-compact v-chip--size-default v-chip--variant-tonal soc\" draggable=\"false\"><span class=\"v-chip__underlay\"></span><div class=\"v-chip__content\" data-no-activator=\"\">{text}</div></span>";
    }

    public static string GetAnchorString(string iconString, string type, int id, string title, string text)
    {
        return $"{iconString} <a href=\"/{type}/{id}\" title=\"{title}\">{text}</a>";
    }

    internal static string GetAnchorCurrentString(string iconString, string title, string text)
    {
        return $"{iconString} <a title=\"{title}\">{text}</a>";
    }

    /// <summary>
    /// Creates color with corrected brightness.
    /// </summary>
    /// <param name="color">Color to correct.</param>
    /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
    /// Negative values produce darker colors.</param>
    /// <returns>
    /// Corrected <see cref="Color"/> structure.
    /// </returns>
    public static Color ChangeColorBrightness(Color color, float correctionFactor)
    {
        float red = color.R;
        float green = color.G;
        float blue = color.B;

        if (correctionFactor < 0)
        {
            correctionFactor = 1 + correctionFactor;
            red *= correctionFactor;
            green *= correctionFactor;
            blue *= correctionFactor;
        }
        else
        {
            red = (255 - red) * correctionFactor + red;
            green = (255 - green) * correctionFactor + green;
            blue = (255 - blue) * correctionFactor + blue;
        }

        return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
    }
}
