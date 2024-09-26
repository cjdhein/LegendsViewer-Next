using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class DanceForm : ArtForm
{
    public static string Icon = "<i class=\"fa fa-fw fa-street-view\"></i>";

    public DanceForm(List<Property> properties, World world)
        : base(properties, world)
    {
        FormType = FormType.Dance;
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        if (link)
        {
            string linkedString = "";
            if (pov != this)
            {
                string title = "Dance Form";
                title += "&#13";
                title += "Events: " + Events.Count;

                linkedString = Icon + "<a href=\"danceform#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
            }
            else
            {
                linkedString = Icon + HtmlStyleUtil.CurrentDwarfObject(Name);
            }
            return linkedString;
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
