using System.Drawing;
using System.Text.Json.Serialization;
using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class Site : WorldObject, IHasCoordinates
{
    public string Icon = "<i class=\"fa fa-fw fa-home\"></i>";

    public string Name { get; set; }
    public string Type { get; set; }

    [JsonIgnore]
    public WorldRegion? Region { get; set; }
    public string? RegionToLink => Region?.ToLink(true, this);

    public SiteType SiteType { get; set; }
    public string UntranslatedName { get; set; }
    public List<Location> Coordinates { get; set; } = [];
    public Rectangle Rectangle { get; set; }
    public bool HasStructures { get; set; }

    [JsonIgnore]
    public List<Structure> Structures { get; set; }
    public List<string> StructuresLinks => Structures.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<EventCollection> Warfare { get; set; }

    [JsonIgnore]
    public List<Battle> Battles { get => Warfare.OfType<Battle>().ToList(); set { } }
    public List<string> BattleLinks => Battles.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<SiteConquered> Conquerings { get => Warfare.OfType<SiteConquered>().ToList(); set { } }
    public List<string> ConqueringLinks => Conquerings.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<HistoricalFigure> RelatedHistoricalFigures { get; set; }
    public List<string> RelatedHistoricalFigureLinks => RelatedHistoricalFigures.ConvertAll(x => x.ToLink(true, this));

    public List<OwnerPeriod> OwnerHistory { get; set; }

    public List<SiteProperty> SiteProperties { get; set; }

    [JsonIgnore]
    public Entity? CurrentOwner
    {
        get
        {
            return OwnerHistory.Any(site => site.EndYear == -1) ? OwnerHistory.First(site => site.EndYear == -1).Owner : null;
        }
        set { }
    }
    public string? CurrentOwnerToLink => CurrentOwner?.ToLink(true, this);

    [JsonIgnore]
    public List<Site> Connections { get; set; }
    public List<string> ConnectionLinks => Connections.ConvertAll(x => x.ToLink(true, this));

    public List<Population> Populations { get; set; }

    public List<Official> Officials { get; set; }
    public List<string> Deaths
    {
        get
        {
            List<string> deaths = [.. NotableDeaths.Select(death => death.Race.Id)];

            foreach (Battle.Squad squad in Battles.SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)).ToList())
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
    public List<string> NotableDeathLinks => NotableDeaths.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<BeastAttack> BeastAttacks { get; set; }
    public List<string> BeastAttackLinks => BeastAttacks.ConvertAll(x => x.ToLink(true, this));

    public class Official
    {
        [JsonIgnore]
        public HistoricalFigure HistoricalFigure;
        public string? HistoricalFigureToLink => HistoricalFigure?.ToLink(true);

        public string Position;

        public Official(HistoricalFigure historicalFigure, string position)
        {
            HistoricalFigure = historicalFigure;
            Position = position;
        }
    }

    public string SiteMapPath { get; set; }

    public Site(List<Property> properties, World world)
        : base(properties, world)
    {
        Type = Name = UntranslatedName = "";
        Warfare = [];
        OwnerHistory = [];
        Connections = [];
        Populations = [];
        Officials = [];
        BeastAttacks = [];
        Structures = [];
        RelatedHistoricalFigures = [];
        SiteProperties = [];

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "type":
                    Type = Formatting.InitCaps(property.Value);
                    switch (property.Value)
                    {
                        case "cave": SiteType = SiteType.Cave; break;
                        case "fortress": SiteType = SiteType.Fortress; break;
                        case "forest retreat": SiteType = SiteType.ForestRetreat; break;
                        case "dark fortress": SiteType = SiteType.DarkFortress; break;
                        case "town": SiteType = SiteType.Town; break;
                        case "hamlet": SiteType = SiteType.Hamlet; break;
                        case "vault": SiteType = SiteType.Vault; break;
                        case "dark pits": SiteType = SiteType.DarkPits; break;
                        case "hillocks": SiteType = SiteType.Hillocks; break;
                        case "tomb": SiteType = SiteType.Tomb; break;
                        case "tower": SiteType = SiteType.Tower; break;
                        case "mountain halls": SiteType = SiteType.MountainHalls; break;
                        case "camp": SiteType = SiteType.Camp; break;
                        case "lair": SiteType = SiteType.Lair; break;
                        case "labyrinth": SiteType = SiteType.Labyrinth; break;
                        case "shrine": SiteType = SiteType.Shrine; break;
                        case "important location": SiteType = SiteType.ImportantLocation; break;
                        case "fort": SiteType = SiteType.Fort; break;
                        case "monastery": SiteType = SiteType.Monastery; break;
                        case "castle": SiteType = SiteType.Castle; break;
                        default:
                            property.Known = false;
                            break;
                    }
                    break;
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "coords": Coordinates.Add(Formatting.ConvertToLocation(property.Value)); break;
                case "structures":
                    HasStructures = true;
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (Property subProperty in property.SubProperties)
                        {
                            subProperty.Known = true;
                            Structures.Add(new Structure(subProperty.SubProperties, world, this));
                        }
                    }
                    break;
                case "civ_id":
                case "cur_owner_id": property.Known = true; break;
                case "rectangle":
                    char[] delimiterChars = { ':', ',' };
                    string[] rectArray = property.Value.Split(delimiterChars);
                    if (rectArray.Length == 4)
                    {
                        int x0 = Convert.ToInt32(rectArray[0]);
                        int y0 = Convert.ToInt32(rectArray[1]);
                        int x1 = Convert.ToInt32(rectArray[2]);
                        int y1 = Convert.ToInt32(rectArray[3]);
                        Rectangle = new Rectangle(x0, y0, x1 - x0, y1 - y0);
                    }
                    else
                    {
                        property.Known = false;
                    }
                    break;
                case "site_properties":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (Property subProperty in property.SubProperties)
                        {
                            subProperty.Known = true;
                            SiteProperties.Add(new SiteProperty(subProperty.SubProperties, world, this));
                        }
                    }
                    break;
            }
        }
        SetIconByType(SiteType);
    }

    private void SetIconByType(SiteType siteType)
    {
        switch (siteType)
        {
            case SiteType.Cave:
                Icon = "<i class=\"fa fa-fw fa-circle\"></i>";
                break;
            case SiteType.Fortress:
                Icon = "<i class=\"fa fa-fw fa-fort-awesome\"></i>";
                break;
            case SiteType.ForestRetreat:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-tree-deciduous\"></i>";
                break;
            case SiteType.DarkFortress:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-compressed fa-rotate-90\"></i>";
                break;
            case SiteType.Town:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-home\"></i>";
                break;
            case SiteType.Hamlet:
                Icon = "<i class=\"fa fa-fw fa-home\"></i>";
                break;
            case SiteType.Vault:
                Icon = "<i class=\"fa fa-fw fa-key\"></i>";
                break;
            case SiteType.DarkPits:
                Icon = "<i class=\"fa fa-fw fa-chevron-circle-down\"></i>";
                break;
            case SiteType.Hillocks:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-grain\"></i>";
                break;
            case SiteType.Tomb:
                Icon = "<i class=\"fa fa-fw fa-archive fa-flip-vertical\"></i>";
                break;
            case SiteType.Tower:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-tower\"></i>";
                break;
            case SiteType.MountainHalls:
                Icon = "<i class=\"fa fa-fw fa-gg-circle\"></i>";
                break;
            case SiteType.Camp:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-tent\"></i>";
                break;
            case SiteType.Lair:
                Icon = "<i class=\"fa fa-fw fa-database\"></i>";
                break;
            case SiteType.Labyrinth:
                Icon = "<i class=\"fa fa-fw fa-ils fa-rotate-90\"></i>";
                break;
            case SiteType.Shrine:
                Icon = "<i class=\"glyphicon fa-fw glyphicon-screenshot\"></i>";
                break;
            case SiteType.Fort:
                Icon = "<i class=\"fa fa-fw fa-th-list fa-rotate-270\"></i>";
                break;
            case SiteType.Monastery:
                Icon = "<i class=\"fa fa-fw fa-viacoin fa-flip-vertical\"></i>";
                break;
            case SiteType.Castle:
                Icon = "<i class=\"fa fa-fw fa-outdent fa-rotate-90\"></i>";
                break;
        }
    }

    public void AddConnection(Site connection)
    {
        if (!Connections.Contains(connection))
        {
            Connections.Add(connection);
        }
    }

    public override string ToString() { return Name; }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        if (link)
        {
            string title = Type;
            title += "&#13";
            title += "Events: " + Events.Count;

            return pov != this
                ? Icon + "<a href = \"site#" + Id + "\" title=\"" + title + "\">" + Name + "</a>"
                : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
