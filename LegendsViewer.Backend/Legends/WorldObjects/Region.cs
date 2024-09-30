using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class WorldRegion : WorldObject, IRegion
{
    public string Icon = HtmlStyleUtil.GetIconString("map-legend");

    public string Name { get; set; }
    public int? Depth { get; set; }
    public RegionType Type { get; set; }
    public List<string> Deaths
    {
        get
        {
            List<string> deaths = [.. NotableDeaths.Select(death => death.Race.Id)];
            foreach (Battle.Squad squad in Battles.SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)))
            {
                for (int i = 0; i < squad.Deaths; i++)
                {
                    deaths.Add(squad.Race.Id);
                }
            }

            return deaths;
        }
        set { }
    }
    [JsonIgnore]
    public List<HistoricalFigure> NotableDeaths { get => Events.OfType<HfDied>().Select(death => death.HistoricalFigure).ToList(); set { } }

    public List<string> NotableDeathLinks => NotableDeaths.ConvertAll(d => d.ToLink(true, this));

    [JsonIgnore]
    public List<Battle> Battles { get; set; }
    public List<string> BattleLinks => Battles.ConvertAll(b => b.ToLink(true, this));
    public List<Location> Coordinates { get; set; } // legends_plus.xml
    public int SquareTiles => Coordinates.Count;

    [JsonIgnore]
    public List<Site> Sites { get; set; } // legends_plus.xml
    public List<string> SiteLinks => Sites.ConvertAll(s => s.ToLink(true, this));

    [JsonIgnore]
    public List<MountainPeak> MountainPeaks { get; set; } // legends_plus.xml
    public List<string> MountainPeakLinks => MountainPeaks.ConvertAll(m => m.ToLink(true, this));
    public Evilness Evilness { get; set; } // legends_plus.xml

    [JsonIgnore]
    public int ForceId { get; set; } // legends_plus.xml

    [JsonIgnore]
    public HistoricalFigure? Force { get; set; } // legends_plus.xml

    public string? ForceLink => Force?.ToLink(true, this);

    public WorldRegion(List<Property> properties, World world)
        : base(properties, world)
    {
        Name = "UNKNOWN REGION";
        ForceId = -1;
        Battles = [];
        Coordinates = [];
        Sites = [];
        MountainPeaks = [];
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "type":
                    switch (property.Value)
                    {
                        case "Mountains":
                            Type = RegionType.Mountains;
                            break;
                        case "Ocean":
                            Type = RegionType.Ocean;
                            break;
                        case "Tundra":
                            Type = RegionType.Tundra;
                            break;
                        case "Glacier":
                            Type = RegionType.Glacier;
                            break;
                        case "Forest":
                            Type = RegionType.Forest;
                            break;
                        case "Wetland":
                            Type = RegionType.Wetland;
                            break;
                        case "Grassland":
                            Type = RegionType.Grassland;
                            break;
                        case "Desert":
                            Type = RegionType.Desert;
                            break;
                        case "Hills":
                            Type = RegionType.Hills;
                            break;
                        case "Lake":
                            Type = RegionType.Lake;
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
                        if (x > world.Width)
                        {
                            world.Width = x;
                        }
                        if (y > world.Height)
                        {
                            world.Height = y;
                        }
                    }
                    break;
                case "evilness":
                    switch (property.Value)
                    {
                        case "good":
                            Evilness = Evilness.Good;
                            break;
                        case "neutral":
                            Evilness = Evilness.Neutral;
                            break;
                        case "evil":
                            Evilness = Evilness.Evil;
                            break;
                        default:
                            property.Known = false;
                            break;
                    }
                    break;
                case "force_id":
                    ForceId = Convert.ToInt32(property.Value);
                    break;
            }
        }
    }
    public override string ToString() { return Name; }
    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string title = Type.GetDescription();
            title += "&#13";
            title += "Events: " + Events.Count;

            return pov != this
                ? $"{HtmlStyleUtil.GetAnchorString(Icon, "region", Id, title, Name)}"
                : $"{HtmlStyleUtil.GetAnchorString(Icon, "region", Id, title, HtmlStyleUtil.CurrentDwarfObject(Name))}";
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }

    public void Resolve(World world)
    {
        if (ForceId != -1)
        {
            Force = world.GetHistoricalFigure(ForceId);
            if (Force?.RelatedRegions.Contains(this) == false)
            {
                Force.RelatedRegions.Add(this);
            }
        }
    }
}
