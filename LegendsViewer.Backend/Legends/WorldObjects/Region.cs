using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Utilities;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class WorldRegion : WorldObject, IRegion
{
    public string Icon { get; set; } = HtmlStyleUtil.GetIconString("map-legend");

    public int? Depth { get; set; }
    public RegionType RegionType { get; set; }
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
                            RegionType = RegionType.Mountains;
                            Icon = HtmlStyleUtil.GetIconString("image-filter-hdr");
                            break;
                        case "Ocean":
                            RegionType = RegionType.Ocean;
                            Icon = HtmlStyleUtil.GetIconString("tsunami");
                            break;
                        case "Tundra":
                            RegionType = RegionType.Tundra;
                            Icon = HtmlStyleUtil.GetIconString("weather-snowy-heavy");
                            break;
                        case "Glacier":
                            RegionType = RegionType.Glacier;
                            Icon = HtmlStyleUtil.GetIconString("snowflake");
                            break;
                        case "Forest":
                            RegionType = RegionType.Forest;
                            Icon = HtmlStyleUtil.GetIconString("forest-outline");
                            break;
                        case "Wetland":
                            RegionType = RegionType.Wetland;
                            Icon = HtmlStyleUtil.GetIconString("home-flood");
                            break;
                        case "Grassland":
                            RegionType = RegionType.Grassland;
                            Icon = HtmlStyleUtil.GetIconString("grass");
                            break;
                        case "Desert":
                            RegionType = RegionType.Desert;
                            Icon = HtmlStyleUtil.GetIconString("cactus");
                            break;
                        case "Hills":
                            RegionType = RegionType.Hills;
                            Icon = HtmlStyleUtil.GetIconString("image-filter-hdr-outline");
                            break;
                        case "Lake":
                            RegionType = RegionType.Lake;
                            Icon = HtmlStyleUtil.GetIconString("weather-hazy");
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
        Type = RegionType.GetDescription();
        Subtype = Evilness.GetDescription();
    }
    public override string ToString() { return Name; }
    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string title = RegionType.GetDescription();
            title += "&#13";
            title += "Evilness: " + Evilness;
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
