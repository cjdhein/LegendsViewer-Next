using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class PoeticForm : ArtForm
{
    public static string Icon = "<i class=\"fa fa-fw fa-sticky-note-o\"></i>";

    public PoeticForm(List<Property> properties, World world)
        : base(properties, world)
    {
        FormType = FormType.Poetic;
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        if (link)
        {
            string title = "Poetic Form";
            title += "&#13";
            title += "Events: " + Events.Count;

            string linkedString = pov != this
                ? Icon + "<a href=\"poeticform#" + Id + "\" title=\"" + title + "\">" + Name + "</a>"
                : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
            return linkedString;
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
