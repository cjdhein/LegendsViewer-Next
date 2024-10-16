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

    public static string GetCivIconString(string text, string bgColor, string fgColor)
    {
        return $"<span style=\"background-color:{bgColor};color:{fgColor};\" class=\"v-chip v-chip--label v-theme--dark v-chip--density-compact v-chip--size-default v-chip--variant-tonal soc\" draggable=\"false\"><span class=\"v-chip__underlay\"></span><div class=\"v-chip__content\" data-no-activator=\"\">{text}</div></span>";
    }

    public static string GetChipString(string text)
    {
        return $"<span class=\"v-chip v-chip--label v-theme--dark v-chip--density-compact v-chip--size-default v-chip--variant-tonal soc\" draggable=\"false\"><span class=\"v-chip__underlay\"></span><div class=\"v-chip__content\" data-no-activator=\"\">{text}</div></span>";
    }

    public static string GetAnchorString(string iconString, string type, int id, string title, string text)
    {
        return $"{iconString} <a href=\"/{type}/{id}\" title=\"{title}\">{text}</a>";
    }

    internal static string GetAnchorCurrentString(string iconString, string title, string text)
    {
        return $"{iconString} <a title=\"{title}\">{text}</a>";
    }
}
