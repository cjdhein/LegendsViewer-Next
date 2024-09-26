using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class MusicalForm : ArtForm
{
    public static string Icon = "<i class=\"fa fa-fw fa-music\"></i>";

    public MusicalForm(List<Property> properties, World world)
        : base(properties, world)
    {
        FormType = FormType.Musical;
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        if (link)
        {
            string title = "Musical Form";
            title += "&#13";
            title += "Events: " + Events.Count;

            return pov != this
                ? Icon + "<a href=\"musicalform#" + Id + "\" title=\"" + title + "\">" + Name + "</a>"
                : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
