using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class UndergroundRegion : WorldObject, IRegion
{
    public string Icon = "<i class=\"fa fa-fw fa-map\"></i>";

    public int? Depth { get; set; }
    public RegionType Type { get; set; }

    [JsonIgnore]
    public List<Battle> Battles { get; set; } = [];
    public List<string> BattleLinks => Battles.ConvertAll(x => x.ToLink(true, this));

    public List<Location> Coordinates { get; set; } = []; // legends_plus.xml
    public int SquareTiles => Coordinates.Count;
    public UndergroundRegion() { }
    public UndergroundRegion(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "depth": Depth = Convert.ToInt32(property.Value); break;
                case "type":
                    switch (property.Value)
                    {
                        case "cavern":
                            Type = RegionType.Cavern;
                            break;
                        case "underworld":
                            Type = RegionType.Underworld;
                            break;
                        case "magma":
                            Type = RegionType.Magma;
                            break;
                        default:
                            property.Known = false;
                            break;
                    }
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
            }
        }
    }
    public override string ToString() { return Type.GetDescription(); }
    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        string name;
        switch (Type)
        {
            case RegionType.Cavern:
                name = "the depths of the world";
                break;
            case RegionType.Underworld:
                name = "the underworld";
                break;
            case RegionType.Magma:
                name = "the magma seas";
                break;
            default:
                name = $"an underground region ({Type})";
                break;
        }

        if (link)
        {
            string title = Type.GetDescription();
            title += "&#13";
            title += "Events: " + Events.Count;

            return pov != this
                ? Icon + "<a href = \"uregion#" + Id + "\" title=\"" + title + "\">" + name + "</a>"
                : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(name) + "</a>";
        }
        return name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
