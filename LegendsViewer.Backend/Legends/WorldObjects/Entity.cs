using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using LegendsViewer.Backend.Contracts;
using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldLinks;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class Entity : WorldObject, IHasCoordinates
{
    [JsonIgnore]
    public Entity? Parent { get; set; }
    public string? ParentLink => Parent?.ToLink(true, this);

    public CreatureInfo Race { get; set; } = CreatureInfo.Unknown;

    [JsonIgnore]
    public List<HistoricalFigure> Worshipped { get; set; } = [];
    public List<ListItemDto> WorshippedLinks
    {
        get
        {
            var list = new List<ListItemDto>();
            foreach (var deity in Worshipped)
            {
                string associatedSpheres = string.Join(", ", deity.Spheres);
                list.Add(new ListItemDto
                {
                    Title = deity.ToLink(true, this),
                    Subtitle = associatedSpheres
                });
            }
            return list;
        }
    }

    [JsonIgnore]
    public List<string> LeaderTypes { get; set; } = [];

    [JsonIgnore]
    public List<List<HistoricalFigure>> Leaders { get; set; } = [];
    // TODO Leaders API

    public EntityPopulation? EntityPopulation { get; set; }

    public List<Population> Populations { get; set; } = [];

    [JsonIgnore]
    public Structure? OriginStructure { get; set; }
    public string? OriginStructureLink => OriginStructure?.ToLink(true, this);

    [JsonIgnore]
    public List<Entity> Groups { get; set; } = [];
    public List<string> GroupLinks => Groups.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<OwnerPeriod> SiteHistory { get; set; } = [];

    [JsonIgnore]
    public List<Site> CurrentSites => SiteHistory.Where(site => site.EndYear == -1).Select(site => site.Site).ToList();
    public List<string> CurrentSiteLinks => CurrentSites.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<Site> LostSites => SiteHistory.Where(site => site.EndYear >= 0).Select(site => site.Site).ToList();
    public List<string> LostSiteLinks => LostSites.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<Site> Sites => SiteHistory.ConvertAll(site => site.Site);
    public List<string> SiteLinks => Sites.ConvertAll(x => x.ToLink(true, this));

    public List<Honor> Honors { get; set; } = [];

    public EntityType EntityType { get; set; } = EntityType.Unknown; // legends_plus.xml
    public bool IsCiv { get; set; }

    [JsonIgnore]
    public List<EntitySiteLink> EntitySiteLinks { get; set; } = []; // legends_plus.xml

    [JsonIgnore]
    public List<EntityEntityLink> EntityEntityLinks { get; set; } = []; // legends_plus.xml

    [JsonIgnore]
    public List<EntityPosition> EntityPositions { get; set; } = []; // legends_plus.xml

    [JsonIgnore]
    public List<EntityPositionAssignment> EntityPositionAssignments { get; set; } = []; // legends_plus.xml
    public List<ListItemDto> EntityPositionAssignmentsList
    {
        get
        {
            var list = new List<ListItemDto>();
            foreach (EntityPositionAssignment assignment in EntityPositionAssignments)
            {
                EntityPosition? position = EntityPositions.Find(pos => pos.Id == assignment.PositionId);
                if (position == null || assignment.HistoricalFigure == null)
                {
                    continue;
                }
                string positionName = position.GetTitleByCaste(assignment.HistoricalFigure.Caste);
                string positionHolder = assignment.HistoricalFigure.ToLink(true, this);

                list.Add(new ListItemDto
                {
                    Title = positionName,
                    Subtitle = positionHolder
                });

                if (string.IsNullOrEmpty(position.Spouse))
                {
                    continue;
                }
                HistoricalFigureLink? spouseHfLink = assignment.HistoricalFigure.RelatedHistoricalFigures.Find(hfLink => hfLink.Type == HistoricalFigureLinkType.Spouse);
                if (spouseHfLink == null)
                {
                    continue;
                }
                HistoricalFigure? spouse = spouseHfLink.HistoricalFigure;
                if (spouse == null)
                {
                    continue;
                }
                string spousePositionName = position.GetTitleByCaste(spouse.Caste, true);
                string spousePositionHolder = spouse.ToLink(true, this);
                list.Add(new ListItemDto
                {
                    Title = spousePositionName,
                    Subtitle = spousePositionHolder
                });
            }
            return list;
        }
    }

    [JsonIgnore]
    public Location? FormedAt { get; set; }
    [JsonIgnore]
    public List<Location> Coordinates
    {
        get
        {
            List<Location> locations = [];
            locations.AddRange(CurrentSites.SelectMany(s => s.Coordinates));
            if (FormedAt != null)
            {
                locations.Add(FormedAt);
            }
            return locations; // legends_plus.xml
        }
    }

    [JsonIgnore]
    public List<EntityOccasion> Occassions { get; set; } = []; // legends_plus.xml

    public List<string> Weapons { get; set; } = [];
    public string? Profession { get; set; }

    [JsonIgnore]
    public List<War> Wars { get; set; } = [];
    public List<string> WarLinks => Wars.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<War> WarsAttacking => Wars.Where(war => war.Attacker == this).ToList();
    public List<string> WarAttackingLinks => WarsAttacking.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<War> WarsDefending => Wars.Where(war => war.Defender == this).ToList();
    public List<string> WarDefendingLinks => WarsDefending.ConvertAll(x => x.ToLink(true, this));

    public int WarVictories => WarsAttacking.Sum(war => war.AttackerBattleVictories.Count) + WarsDefending.Sum(war => war.DefenderBattleVictories.Count);
    public int WarLosses => WarsAttacking.Sum(war => war.DefenderBattleVictories.Count) + WarsDefending.Sum(war => war.AttackerBattleVictories.Count);
    public int WarKills => WarsAttacking.Sum(war => war.DefenderDeathCount) + WarsDefending.Sum(war => war.AttackerDeathCount);
    public int WarDeaths => WarsAttacking.Sum(war => war.AttackerDeathCount) + WarsDefending.Sum(war => war.DefenderDeathCount);

    [JsonIgnore]
    public List<HistoricalFigure> AllLeaders => Leaders.SelectMany(l => l).ToList();
    public List<string> AllLeaderLinks => AllLeaders.ConvertAll(x => x.ToLink(true, this));

    public List<string> PopulationsAsList
    {
        get
        {
            var populations = new List<string>();
            foreach (var population in Populations)
            {
                for (var i = 0; i < population.Count; i++)
                {
                    populations.Add(population.Race.NamePlural);
                }
            }

            return populations;
        }
    }

    public double WarKillDeathRatio
    {
        get
        {
            if (WarDeaths == 0 && WarKills == 0)
            {
                return 0;
            }

            return WarDeaths == 0 ? double.MaxValue : Math.Round(WarKills / Convert.ToDouble(WarDeaths), 2);
        }
    }

    public Color LineColor { get; set; }

    private string? _icon;

    public new string Icon
    {
        get
        {
            if (string.IsNullOrEmpty(_icon))
            {
                string coloredIcon;
                if (IsCiv)
                {
                    coloredIcon = HtmlStyleUtil.GetCivIconString(Formatting.GetInitials(Name), ColorTranslator.ToHtml(LineColor));
                }
                else if (World != null && World.MainRaces.ContainsKey(Race))
                {
                    Color civilizedPopColor = LineColor;
                    if (civilizedPopColor == Color.Empty)
                    {
                        civilizedPopColor = World.MainRaces.FirstOrDefault(r => r.Key == Race).Value;
                    }
                    coloredIcon = HtmlStyleUtil.GetIconString("account-group", ColorTranslator.ToHtml(civilizedPopColor));
                }
                else
                {
                    coloredIcon = HtmlStyleUtil.GetIconString("account-multiple");
                }
                _icon = coloredIcon;
            }
            return _icon;
        }
        set => _icon = value;
    }

    public Entity(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "race":
                    Race = world.GetCreatureInfo(property.Value);
                    break;
                case "type":
                    switch (property.Value)
                    {
                        case "civilization":
                            EntityType = EntityType.Civilization;
                            break;
                        case "religion":
                            EntityType = EntityType.Religion;
                            break;
                        case "sitegovernment":
                            EntityType = EntityType.SiteGovernment;
                            break;
                        case "nomadicgroup":
                            EntityType = EntityType.NomadicGroup;
                            break;
                        case "outcast":
                            EntityType = EntityType.Outcast;
                            break;
                        case "migratinggroup":
                            EntityType = EntityType.MigratingGroup;
                            break;
                        case "performancetroupe":
                            EntityType = EntityType.PerformanceTroupe;
                            break;
                        case "guild":
                            EntityType = EntityType.Guild;
                            break;
                        case "militaryunit":
                            EntityType = EntityType.MilitaryUnit;
                            break;
                        case "merchantcompany":
                            EntityType = EntityType.MerchantCompany;
                            break;
                        default:
                            EntityType = EntityType.Unknown;
                            property.Known = false;
                            break;
                    }
                    break;
                case "child":
                    property.Known = true; // TODO child entities
                    break;
                case "site_link":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        EntitySiteLinks.Add(new EntitySiteLink(property.SubProperties, world));
                    }

                    break;
                case "entity_link":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (Property subProperty in property.SubProperties)
                        {
                            subProperty.Known = true;
                        }
                    }

                    world.AddEntityEntityLink(this, property);
                    break;
                case "worship_id":
                    var worshippedDeity = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                    if (worshippedDeity != null)
                    {
                        Worshipped.Add(worshippedDeity);
                    }
                    break;
                //case "claims":
                //    string[] coordinateStrings = property.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //    foreach (var coordinateString in coordinateStrings)
                //    {
                //        string[] xYCoordinates = coordinateString.Split(',');
                //        if (xYCoordinates.Length == 2)
                //        {
                //            int x = Convert.ToInt32(xYCoordinates[0]);
                //            int y = Convert.ToInt32(xYCoordinates[1]);
                //            Coordinates.Add(new Location(x, y));
                //        }
                //    }
                //    break;
                case "entity_position":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        EntityPositions.Add(new EntityPosition(property.SubProperties, world));
                    }
                    break;
                case "entity_position_assignment":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        EntityPositionAssignments.Add(new EntityPositionAssignment(property.SubProperties, world));
                    }
                    break;
                case "histfig_id":
                    property.Known = true; // TODO historical figure == living members?
                    break;
                case "occasion":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        Occassions.Add(new EntityOccasion(property.SubProperties, world, this));
                    }
                    break;
                case "honor":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        Honors.Add(new Honor(property.SubProperties, world, this));
                    }
                    break;
                case "weapon":
                    Weapons.Add(property.Value);
                    break;
                case "profession":
                    Profession = property.Value.Replace("_", " ");
                    break;
            }
        }
        Type = EntityType.GetDescription();
        Subtype = Race?.NamePlural ?? string.Empty;
    }
    public override string ToString() { return Name ?? "UNKNOWN"; }

    public bool EqualsOrParentEquals(Entity? entity)
    {
        return entity != null && (this == entity || Parent == entity);
    }

    public string PrintEntity(bool link = true, DwarfObject? pov = null)
    {
        string entityString = ToLink(link, pov);
        if (Parent != null)
        {
            entityString += " of " + Parent.ToLink(link, pov);
        }
        return entityString;
    }

    //TODO: Check and possibly move logic
    public void AddOwnedSite(OwnerPeriod ownerPeriod)
    {
        if (ownerPeriod.StartCause == "UNKNOWN" && SiteHistory.All(s => s.Site != ownerPeriod.Site))
        {
            SiteHistory.Insert(0, ownerPeriod);
        }
        else
        {
            SiteHistory.Add(ownerPeriod);
        }

        if (ownerPeriod.Owner != null && ownerPeriod.Owner != this)
        {
            Groups.Add(ownerPeriod.Owner);
        }

        if (!IsCiv && Parent != null)
        {
            Parent.AddOwnedSite(ownerPeriod);
            Race = Parent.Race;
        }
    }

    public void AddPopulations(List<Population> populations)
    {
        foreach (Population population in populations)
        {
            Population? popMatch = Populations.Find(pop => pop.Race.NamePlural.Equals(population.Race.NamePlural, StringComparison.InvariantCultureIgnoreCase));
            if (popMatch != null)
            {
                popMatch.Count += population.Count;
            }
            else
            {
                Populations.Add(new Population(World, population.Race, population.Count));
            }
        }
        Populations = Populations.OrderByDescending(pop => pop.Count).ToList();
        Parent?.AddPopulations(populations);
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        string name = string.IsNullOrWhiteSpace(Name) ? GetTitle() : Name;
        if (link)
        {
            return pov != this
                ? HtmlStyleUtil.GetAnchorString(Icon, "entity", Id, GetToolTip(), name)
                : HtmlStyleUtil.GetAnchorCurrentString(Icon, GetToolTip(), HtmlStyleUtil.CurrentDwarfObject(name));
        }
        return name;
    }

    private string GetToolTip()
    {
        string title = GetTitle();
        if (Parent != null)
        {
            title += "&#13";
            title += "Part of " + Parent.Name;
        }
        title += "&#13";
        title += "Events: " + Events.Count;
        return title;
    }

    public string GetTitle()
    {
        var title = IsCiv ? "Civilization" : GetTypeAsString();
        if (Race != null && Race != CreatureInfo.Unknown)
        {
            title += " of ";
            title += Race.NamePlural;
        }
        return title;
    }

    private string GetTypeAsString()
    {
        switch (EntityType)
        {
            case EntityType.Civilization:
                return "Civilization";
            case EntityType.NomadicGroup:
                return "Nomadic group";
            case EntityType.MigratingGroup:
                return "Migrating group";
            case EntityType.Outcast:
                return "Collection of outcasts";
            case EntityType.Religion:
                return "Religious group";
            case EntityType.SiteGovernment:
                return "Site government";
            case EntityType.PerformanceTroupe:
                return "Performance troupe";
            case EntityType.MercenaryCompany:
                return "Mercenary company";
            case EntityType.MilitaryUnit:
                return "Mercenary order";
            case EntityType.Guild:
                return "Guild";
            case EntityType.MerchantCompany:
                return "Merchant company";
            default:
                return "Group";
        }
    }

    public string GetSummary(bool link = true, DwarfObject? pov = null)
    {
        string summary = string.Empty;
        summary += ToLink(link, pov);
        summary += " was a ";
        summary += GetTypeAsString().ToLower();
        if (Race != CreatureInfo.Unknown)
        {
            summary += " of " + Race.NamePlural.ToLower();
        }
        if (Parent != null)
        {
            summary += " of " + Parent.ToLink(link, pov);
        }

        switch (EntityType)
        {
            case EntityType.Religion:
                if (Worshipped.Count > 0)
                {
                    summary += " centered around the worship of " + Worshipped[0].ToLink(link, pov);
                }
                break;
            case EntityType.MilitaryUnit:
                bool isWorshipping = false;
                if (Worshipped.Count > 0)
                {
                    summary += " devoted to the worship of " + Worshipped[0].ToLink(link, pov);
                    isWorshipping = true;
                }
                if (Weapons.Count > 0)
                {
                    if (isWorshipping)
                    {
                        summary += " and";
                    }
                    summary += " dedicated to the mastery of the " + string.Join(", the ", Weapons);
                }
                break;
            case EntityType.Guild:
                summary += " of " + Profession + "s";
                break;
        }
        summary += ".";
        return summary;
    }

    public override string GetIcon()
    {
        return Icon;
    }

    public void SetParent(Entity parent)
    {
        if (parent == null || parent == this)
        {
            return;
        }

        if (parent.Name == Name)
        {
            return;
        }

        Parent = parent;
    }
}
