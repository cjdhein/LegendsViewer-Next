using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class Landmass : WorldObject, IHasCoordinates
{
    public static readonly string Icon = "<i class=\"fa fa-fw fa-life-ring\"></i>";

    public string Name { get; set; } // legends_plus.xml
    public List<Location> Coordinates { get; set; } // legends_plus.xml

    public Landmass(List<Property> properties, World world)
        : base(properties, world)
    {
        Name = "Untitled";
        Coordinates = [];
        string[] coordinateStrings;
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "coord_1":
                    coordinateStrings = property.Value.Split(new[] { '|' },
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (var coordinateString in coordinateStrings)
                    {
                        string[] xYCoordinates = coordinateString.Split(',');
                        int x = Convert.ToInt32(xYCoordinates[0]);
                        int y = Convert.ToInt32(xYCoordinates[1]);
                        Coordinates.Add(new Location(x, y));
                    }
                    break;
                case "coord_2":
                    coordinateStrings = property.Value.Split(new[] { '|' },
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (var coordinateString in coordinateStrings)
                    {
                        string[] xYCoordinates = coordinateString.Split(',');
                        int x = Convert.ToInt32(xYCoordinates[0]);
                        int y = Convert.ToInt32(xYCoordinates[1]);
                        Coordinates.Add(new Location(x, y));
                    }
                    break;
            }
        }
    }

    public override string ToString() { return Name; }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        if (link)
        {
            string linkedString = "";
            if (pov != this)
            {
                string title = "";
                title += "Landmass";
                title += "&#13";
                title += "Events: " + Events.Count;

                linkedString = Icon + "<a href = \"landmass#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
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
