using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class MountainPeak : WorldObject, IHasCoordinates
{
    public string Name { get; set; } // legends_plus.xml
    public WorldRegion Region { get; set; }
    public List<Location> Coordinates { get; set; } // legends_plus.xml
    public int Height { get; set; } // legends_plus.xml
    public string HeightMeter { get => Height * 3 + " m"; set { } } // legends_plus.xml
    public bool IsVolcano { get; set; }

    public string TypeAsString => IsVolcano ? "Volcano" : "Mountain";

    public string Icon = "<i class=\"fa fa-fw fa-wifi fa-flip-vertical\"></i>";

    public MountainPeak(List<Property> properties, World world)
        : base(properties, world)
    {
        Name = "Untitled";
        Coordinates = [];

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name":
                    Name = Formatting.InitCaps(property.Value);
                    break;
                case "coords":
                    string[] coordinateStrings = property.Value.Split(new[] { '|' },
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (var coordinateString in coordinateStrings)
                    {
                        string[] xYCoordinates = coordinateString.Split(',');
                        int x = Convert.ToInt32(xYCoordinates[0]);
                        int y = Convert.ToInt32(xYCoordinates[1]);
                        Coordinates.Add(new Location(x, y));
                    }
                    break;
                case "height":
                    Height = Convert.ToInt32(property.Value);
                    break;
                case "is_volcano":
                    IsVolcano = true;
                    property.Known = true;
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
                title += IsVolcano ? "Volcano" : "Mountain Peak";
                title += "&#13";
                title += "Events: " + Events.Count;

                linkedString = Icon + "<a href = \"mountainpeak#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
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
