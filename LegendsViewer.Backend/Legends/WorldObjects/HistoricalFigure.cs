using System.Globalization;
using System.Text.Json.Serialization;
using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldLinks;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class HistoricalFigure : WorldObject
{
    private static readonly List<string> KnownEntitySubProperties = ["entity_id", "link_strength", "link_type", "position_profile_id", "start_year", "end_year"];
    private static readonly List<string> KnownSiteLinkSubProperties = ["link_type", "site_id", "sub_id", "entity_id", "occupation_id"];
    private static readonly List<string> KnownEntitySquadLinkProperties = ["squad_id", "squad_position", "entity_id", "start_year", "end_year"];

    public static readonly string ForceNatureIcon = HtmlStyleUtil.GetIconString("leaf");
    public static readonly string DeityIcon = HtmlStyleUtil.GetIconString("weather-sunset");
    public static readonly string NeuterIcon = HtmlStyleUtil.GetIconString("gender-non-binary");
    public static readonly string FemaleIcon = HtmlStyleUtil.GetIconString("gender-female");
    public static readonly string MaleIcon = HtmlStyleUtil.GetIconString("gender-male");

    public static readonly HistoricalFigure Unknown = new();

    private string ShortName
    {
        get
        {
            if (string.IsNullOrEmpty(_shortName))
            {
                _shortName = Name.IndexOf(" ", StringComparison.Ordinal) >= 2 && !Name.StartsWith("The ")
                    ? Name.Substring(0, Name.IndexOf(" ", StringComparison.Ordinal))
                    : Name;
            }
            return _shortName;
        }
    }

    private string RaceString
    {
        get
        {
            if (string.IsNullOrEmpty(_raceString))
            {
                _raceString = GetRaceString();
            }
            return _raceString;
        }
    }

    public string TitleRaceString
    {
        get
        {
            if (string.IsNullOrEmpty(_TitleRaceString))
            {
                _TitleRaceString = GetRaceString();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                _TitleRaceString = textInfo.ToTitleCase(_TitleRaceString);
            }
            return _TitleRaceString;
        }
    }

    private string Title
    {
        get
        {
            if (string.IsNullOrEmpty(_title))
            {
                _title = GetAnchorTitle();
            }
            return _title;
        }
    }

    public CreatureInfo Race { get; set; }
    public string? Caste { get; set; }
    public string? AssociatedType { get; set; }
    public string? PreviousRace { get; set; }
    public int EntityPopulationId { get; set; }
    public EntityPopulation? EntityPopulation { get; set; }
    public HfState CurrentState { get; set; }
    public List<int> UsedIdentityIds { get; set; }
    public int CurrentIdentityId { get; set; }

    [JsonIgnore]
    public List<Artifact> HoldingArtifacts { get; set; }
    public List<string> HoldingArtifactLinks => HoldingArtifacts.ConvertAll(x => x.ToLink(true, this));

    public List<State> States { get; set; }

    public List<CreatureType> CreatureTypes { get; set; }
    public List<HistoricalFigureLink> RelatedHistoricalFigures { get; set; }
    public List<SiteProperty> SiteProperties { get; set; }
    public List<EntityLink> RelatedEntities { get; set; }
    public List<EntityReputation> Reputations { get; set; }
    public List<RelationshipProfileHf> RelationshipProfiles { get; set; }

    [JsonIgnore]
    public Dictionary<int, RelationshipProfileHf> RelationshipProfilesOfIdentities { get; set; } // TODO not used in Legends Mode
    public List<SiteLink> RelatedSites { get; set; }

    [JsonIgnore]
    public List<WorldRegion> RelatedRegions { get; set; }
    public List<string> RelatedRegionLinks => RelatedRegions.ConvertAll(x => x.ToLink(true, this));

    public List<Skill> Skills { get; set; }
    public List<VagueRelationship> VagueRelationships { get; set; }

    [JsonIgnore]
    public List<Structure> DedicatedStructures { get; set; }
    public List<string> DedicatedStructuresLinks => DedicatedStructures.ConvertAll(x => x.ToLink(true, this));

    public int Age { get; set; }
    public int Appeared { get; set; }
    public int BirthYear { get; set; }
    public int BirthSeconds72 { get; set; }
    public int DeathYear { get; set; }
    public int DeathSeconds72 { get; set; }
    public DeathCause DeathCause { get; set; }
    public List<string> ActiveInteractions { get; set; }
    public List<string> InteractionKnowledge { get; set; }
    public string Goal { get; set; }
    public string Interaction { get; set; }

    [JsonIgnore]
    public HistoricalFigure? LineageCurseParent { get; set; }
    public string? LineageCurseParentToLink => LineageCurseParent?.ToLink(true, this);

    [JsonIgnore]
    public List<HistoricalFigure> LineageCurseChilds { get; set; }
    public List<string> LineageCurseChildLinks => LineageCurseChilds.ConvertAll(x => x.ToLink(true, this));

    public List<string> JourneyPets { get; set; }

    [JsonIgnore]
    public List<HfDied> NotableKills { get; set; }

    [JsonIgnore]
    public List<HistoricalFigure> HFKills => NotableKills.ConvertAll(kill => kill.HistoricalFigure);
    public List<string> HFKillLinks => HFKills.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<HistoricalFigure> SnatchedHfs
    {
        get => Events
            .OfType<HfAbducted>()
            .Where(abduction => abduction.Snatcher == this && abduction.Target != null)
            .Select(abduction => abduction.Target!)
            .ToList();
        set { }
    }
    public List<string> SnatchedHfLinks => SnatchedHfs.ConvertAll(x => x.ToLink(true, this));

    public List<string> Spheres { get; set; }

    [JsonIgnore]
    public List<Battle> Battles { get; set; }
    public List<string> BattleLinks => Battles.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<Battle> BattlesAttacking => Battles.Where(battle => battle.NotableAttackers.Contains(this)).ToList();
    public List<string> BattlesAttackingLinks => BattlesAttacking.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<Battle> BattlesDefending => Battles.Where(battle => battle.NotableDefenders.Contains(this)).ToList();
    public List<string> BattlesDefendingLinks => BattlesDefending.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public List<Battle> BattlesNonCombatant => Battles.Where(battle => battle.NonCombatants.Contains(this)).ToList();
    public List<string> BattlesNonCombatantLinks => BattlesNonCombatant.ConvertAll(x => x.ToLink(true, this));

    public List<Position> Positions { get; set; }

    [JsonIgnore]
    public Entity? WorshippedBy { get; set; }
    public string? WorshippedByToLink => WorshippedBy?.ToLink(true);

    [JsonIgnore]
    public List<BeastAttack> BeastAttacks { get; set; }
    public List<string> BeastAttackLinks => BeastAttacks.ConvertAll(x => x.ToLink(true, this));

    public HonorEntity HonorEntity { get; set; }
    public List<IntrigueActor> IntrigueActors { get; set; }
    public List<IntriguePlot> IntriguePlots { get; set; }
    public List<Identity> Identities { get; set; } = [];

    public bool Alive
    {
        get => DeathYear == -1;
        set { }
    }

    public bool Deity { get; set; }
    public bool Skeleton { get; set; }
    public bool Force { get; set; }
    public bool Zombie { get; set; }
    public bool Ghost { get; set; }
    public bool Animated { get; set; }
    public string AnimatedType { get; set; }
    public bool Adventurer { get; set; }
    public string BreedId { get; set; }

    private string _shortName;
    private string _raceString;
    private string _TitleRaceString;
    private string _title;

    public HistoricalFigure()
    {
        Initialize();
        Name = "an unknown creature";
        Race = CreatureInfo.Unknown;
        Caste = "UNKNOWN";
        AssociatedType = "UNKNOWN";
    }

    public override string ToString()
    {
        return Name;
    }

    public HistoricalFigure(List<Property> properties, World world)
        : base(properties, world)
    {
        Initialize();
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "appeared": Appeared = Convert.ToInt32(property.Value); break;
                case "birth_year": BirthYear = Convert.ToInt32(property.Value); break;
                case "birth_seconds72": BirthSeconds72 = Convert.ToInt32(property.Value); break;
                case "death_year": DeathYear = Convert.ToInt32(property.Value); break;
                case "death_seconds72": DeathSeconds72 = Convert.ToInt32(property.Value); break;
                case "name": Name = Formatting.InitCaps(property.Value.Replace("'", "`")); break;
                case "race": Race = world.GetCreatureInfo(property.Value); break;
                case "caste": Caste = Formatting.InitCaps(property.Value); break;
                case "associated_type": AssociatedType = Formatting.InitCaps(property.Value); break;
                case "deity": Deity = true; property.Known = true; break;
                case "skeleton": Skeleton = true; property.Known = true; break;
                case "force": Force = true; property.Known = true; Race = world.GetCreatureInfo("Force"); break;
                case "zombie": Zombie = true; property.Known = true; break;
                case "ghost": Ghost = true; property.Known = true; break;
                case "hf_link": //Will be processed after all HFs have been loaded
                    world.AddHFtoHfLink(this, property);
                    property.Known = true;
                    List<string> knownSubProperties = ["hfid", "link_strength", "link_type"];
                    if (property.SubProperties != null)
                    {
                        foreach (string subPropertyName in knownSubProperties)
                        {
                            Property subProperty = property.SubProperties.Find(property1 => property1.Name == subPropertyName);
                            if (subProperty != null)
                            {
                                subProperty.Known = true;
                            }
                        }
                    }

                    break;

                case "entity_link":
                case "entity_former_position_link":
                case "entity_position_link":
                    world.AddHFtoEntityLink(this, property);
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (string subPropertyName in KnownEntitySubProperties)
                        {
                            Property subProperty = property.SubProperties.Find(property1 => property1.Name == subPropertyName);
                            if (subProperty != null)
                            {
                                subProperty.Known = true;
                            }
                        }
                    }

                    break;

                case "entity_reputation":
                    world.AddReputation(this, property);
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (string subPropertyName in Reputation.KnownReputationSubProperties)
                        {
                            Property subProperty = property.SubProperties.Find(property1 => property1.Name == subPropertyName);
                            if (subProperty != null)
                            {
                                subProperty.Known = true;
                            }
                        }
                    }

                    break;

                case "entity_squad_link":
                case "entity_former_squad_link":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (string subPropertyName in KnownEntitySquadLinkProperties)
                        {
                            Property subProperty = property.SubProperties.Find(property1 => property1.Name == subPropertyName);
                            if (subProperty != null)
                            {
                                subProperty.Known = true;
                            }
                        }
                    }

                    break;

                case "relationship_profile_hf":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        RelationshipProfiles.Add(new RelationshipProfileHf(property.SubProperties, RelationShipProfileType.Unknown));
                    }
                    break;
                case "relationship_profile_hf_identity":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        var relationshipProfileHfIdentity = new RelationshipProfileHf(property.SubProperties, RelationShipProfileType.Identity);
                        RelationshipProfilesOfIdentities.Add(relationshipProfileHfIdentity.Id, relationshipProfileHfIdentity);
                    }
                    break;

                case "relationship_profile_hf_visual":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        RelationshipProfiles.Add(new RelationshipProfileHf(property.SubProperties, RelationShipProfileType.Visual));
                    }
                    break;

                case "relationship_profile_hf_historical":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        RelationshipProfiles.Add(new RelationshipProfileHf(property.SubProperties, RelationShipProfileType.Historical));
                    }
                    break;

                case "site_link":
                    world.AddHFtoSiteLink(this, property);
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (string subPropertyName in KnownSiteLinkSubProperties)
                        {
                            Property subProperty = property.SubProperties.Find(property1 => property1.Name == subPropertyName);
                            if (subProperty != null)
                            {
                                subProperty.Known = true;
                            }
                        }
                    }

                    break;

                case "hf_skill":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        var skill = new Skill(property.SubProperties);
                        Skills.Add(skill);
                    }
                    break;

                case "active_interaction": ActiveInteractions.Add(string.Intern(property.Value)); break;
                case "interaction_knowledge": InteractionKnowledge.Add(string.Intern(property.Value)); break;
                case "animated": Animated = true; property.Known = true; break;
                case "animated_string": if (AnimatedType != "") { throw new Exception("Animated Type already exists."); } AnimatedType = Formatting.InitCaps(property.Value); break;
                case "journey_pet":
                    var creatureInfo = world.GetCreatureInfo(property.Value);
                    if (creatureInfo != CreatureInfo.Unknown)
                    {
                        JourneyPets.Add(creatureInfo.NameSingular);
                    }
                    else
                    {
                        JourneyPets.Add(Formatting.FormatRace(property.Value));
                    }
                    break;
                case "goal": Goal = Formatting.InitCaps(property.Value); break;
                case "sphere": Spheres.Add(property.Value); break;
                case "current_identity_id": CurrentIdentityId = Convert.ToInt32(property.Value); break;
                case "used_identity_id": UsedIdentityIds.Add(Convert.ToInt32(property.Value)); break;
                case "ent_pop_id": EntityPopulationId = Convert.ToInt32(property.Value); break;
                case "holds_artifact":
                    var artifact = world.GetArtifact(Convert.ToInt32(property.Value));
                    HoldingArtifacts.Add(artifact);
                    artifact.Holder = this;
                    break;

                case "adventurer":
                    Adventurer = true;
                    property.Known = true;
                    break;

                case "breed_id":
                    BreedId = property.Value;
                    if (!string.IsNullOrWhiteSpace(BreedId))
                    {
                        if (world.Breeds.ContainsKey(BreedId))
                        {
                            world.Breeds[BreedId].Add(this);
                        }
                        else
                        {
                            world.Breeds.Add(BreedId, [this]);
                        }
                    }
                    break;

                case "sex": property.Known = true; break;
                case "site_property":
                    // is resolved in SiteProperty.Resolve()
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        foreach (Property subProperty in property.SubProperties)
                        {
                            switch (subProperty.Name)
                            {
                                case "site_id":
                                case "property_id":
                                    subProperty.Known = true;
                                    break;
                            }
                        }
                    }
                    break;

                case "vague_relationship":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        VagueRelationships.Add(new VagueRelationship(property.SubProperties));
                    }
                    break;

                case "honor_entity":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        HonorEntity = new HonorEntity(property.SubProperties, world);
                    }
                    break;

                case "intrigue_actor":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        IntrigueActors.Add(new IntrigueActor(property.SubProperties));
                    }
                    break;

                case "intrigue_plot":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        IntriguePlots.Add(new IntriguePlot(property.SubProperties));
                    }
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            Name = !string.IsNullOrWhiteSpace(AnimatedType) ? Formatting.InitCaps(AnimatedType) : "(Unnamed)";
        }
        Type = Race?.NameSingular ?? string.Empty;
        Subtype = Caste ?? string.Empty;
        if (Adventurer)
        {
            world.AddPlayerRelatedDwarfObjects(this);
        }
    }

    private void Initialize()
    {
        Appeared = BirthYear = BirthSeconds72 = DeathYear = DeathSeconds72 = EntityPopulationId = -1;
        Name = Caste = AssociatedType = "";
        Race = CreatureInfo.Unknown;
        DeathCause = DeathCause.None;
        CurrentState = HfState.None;
        NotableKills = [];
        Battles = [];
        Positions = [];
        Spheres = [];
        BeastAttacks = [];
        States = [];
        CreatureTypes = [];
        RelatedHistoricalFigures = [];
        RelatedEntities = [];
        Reputations = [];
        RelationshipProfiles = [];
        RelationshipProfilesOfIdentities = [];
        RelatedSites = [];
        RelatedRegions = [];
        Skills = [];
        AnimatedType = "";
        Goal = "";
        ActiveInteractions = [];
        PreviousRace = "";
        InteractionKnowledge = [];
        JourneyPets = [];
        HoldingArtifacts = [];
        LineageCurseChilds = [];
        DedicatedStructures = [];
        UsedIdentityIds = [];
        SiteProperties = [];
        VagueRelationships = [];
        IntrigueActors = [];
        IntriguePlots = [];
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (this == Unknown)
        {
            return Name;
        }

        if (link)
        {
            string icon = GetIcon();
            if (pov == null || pov != this)
            {
                if (pov != null && pov.GetType() == typeof(BeastAttack) && (pov as BeastAttack)?.Beast == this) //Highlight Beast when printing Beast Attack Log
                {
                    return $"{HtmlStyleUtil.GetAnchorString(icon, "hf", Id, Title, ShortName)}";
                }

                return worldEvent != null
                    ? $"the {GetRaceStringByWorldEvent(worldEvent)} {HtmlStyleUtil.GetAnchorString(icon, "hf", Id, Title, Name)}"
                    : $"the {RaceString} {HtmlStyleUtil.GetAnchorString(icon, "hf", Id, Title, Name)}";
            }
            return $"{HtmlStyleUtil.GetAnchorString("", "hf", Id, Title, HtmlStyleUtil.CurrentDwarfObject(ShortName))}";
        }
        if (pov == null || pov != this)
        {
            return worldEvent != null ? $"{GetRaceStringByWorldEvent(worldEvent)} {Name}" : $"{RaceString} {Name}";
        }
        return ShortName;
    }

    public override string GetIcon()
    {
        if (Force)
        {
            return ForceNatureIcon;
        }
        if (Deity)
        {
            return DeityIcon;
        }
        if (Caste == "Female")
        {
            return FemaleIcon;
        }
        if (Caste == "Male")
        {
            return MaleIcon;
        }
        return Caste == "Default" ? NeuterIcon : "";
    }

    private string GetAnchorTitle()
    {
        string title = "";
        if (Positions.Count > 0)
        {
            title += GetLastNoblePosition();
            title += "&#13";
        }
        if (!string.IsNullOrWhiteSpace(AssociatedType) && AssociatedType != "Standard")
        {
            title += AssociatedType;
            title += "&#13";
        }
        title += !string.IsNullOrWhiteSpace(Caste) && Caste != "Default" ? Caste + " " : "";
        title += Formatting.InitCaps(RaceString);
        if (!Deity && !Force)
        {
            title += " (" + BirthYear + " - " + (DeathYear == -1 ? "Present" : DeathYear.ToString()) + ")";
        }
        title += "&#13";
        title += "Events: " + Events.Count;
        return title;
    }

    public string GetLastNoblePosition()
    {
        string title = "";
        if (Positions.Count > 0)
        {
            string positionName = "";
            var hfposition = Positions.Last();
            EntityPosition position = hfposition.Entity.EntityPositions.Find(pos => string.Equals(pos.Name, hfposition.Title, StringComparison.OrdinalIgnoreCase));
            positionName = position != null ? position.GetTitleByCaste(Caste) : hfposition.Title;
            title += (hfposition.Ended == -1 ? "" : "Former ") + positionName + " of " + hfposition.Entity.Name;
        }
        return title;
    }

    private List<Assignment> _assignments;
    public string GetLastAssignmentString()
    {
        if (_assignments?.Count > 0)
        {
            Assignment lastAssignment = _assignments.Last();
            if (lastAssignment.Ended != -1)
            {
                return $"Former {lastAssignment.Title}";
            }
            return lastAssignment.Title;
        }
        var lastAssignmentString = GetLastNoblePosition();
        if (!string.IsNullOrEmpty(lastAssignmentString))
        {
            return lastAssignmentString;
        }
        _assignments = [];
        var relevantEvents = Events.Where(e => e is ChangeHfJob || e is AddHfEntityLink);
        if (relevantEvents.Any())
        {
            foreach (var relevantEvent in relevantEvents)
            {
                var lastAssignment = _assignments.LastOrDefault();
                if (relevantEvent is ChangeHfJob changeHfJobEvent)
                {
                    if (!string.Equals(changeHfJobEvent.NewJob, "UNKNOWN JOB", StringComparison.OrdinalIgnoreCase))
                    {
                        if (lastAssignment != null)
                        {
                            lastAssignment.Ended = changeHfJobEvent.Year;
                        }
                        _assignments.Add(new Assignment(changeHfJobEvent.Site, changeHfJobEvent.Year, -1, changeHfJobEvent.NewJob));
                    }
                    else if (lastAssignment != null && changeHfJobEvent.OldJob == lastAssignment.Title)
                    {
                        lastAssignment.Ended = changeHfJobEvent.Year;
                    }
                }
                else if (relevantEvent is AddHfEntityLink addHfEntityLinkEvent && (addHfEntityLinkEvent.LinkType == HfEntityLinkType.Squad || addHfEntityLinkEvent.LinkType == HfEntityLinkType.Position))
                {
                    EntityPosition position = addHfEntityLinkEvent.Entity.EntityPositions.Find(pos => string.Equals(pos.Name, addHfEntityLinkEvent.Position, StringComparison.OrdinalIgnoreCase) || pos.Id == addHfEntityLinkEvent.PositionId);
                    if (position != null)
                    {
                        if (lastAssignment != null)
                        {
                            lastAssignment.Ended = addHfEntityLinkEvent.Year;
                        }
                        string positionName = position.GetTitleByCaste(addHfEntityLinkEvent.HistoricalFigure?.Caste);
                        _assignments.Add(new Assignment(addHfEntityLinkEvent.Entity, addHfEntityLinkEvent.Year, -1, positionName));
                    }
                    else if (!string.IsNullOrWhiteSpace(addHfEntityLinkEvent.Position))
                    {
                        if (lastAssignment != null)
                        {
                            lastAssignment.Ended = addHfEntityLinkEvent.Year;
                        }
                        _assignments.Add(new Assignment(addHfEntityLinkEvent.Entity, addHfEntityLinkEvent.Year, -1, addHfEntityLinkEvent.Position));
                    }
                }
            }
        }
        if (_assignments?.Count > 0)
        {
            Assignment lastAssignment = _assignments.Last();
            if (lastAssignment.Ended != -1)
            {
                return $"Former {lastAssignment.Title}";
            }
            return lastAssignment.Title;
        }
        return AssociatedType;
    }

    public string GetHighestSkillAsString()
    {
        if (Skills.Count > 0)
        {
            var highestSkill = Skills.OrderBy(skill => skill.Points).Last();
            var highestSkillDescription = SkillDictionary.LookupSkill(highestSkill);
            return $"{highestSkillDescription.Rank} {highestSkillDescription.Name}";
        }
        return string.Empty;
    }

    public string GetSpheresAsString()
    {
        if (Spheres == null)
        {
            return string.Empty;
        }
        string spheres = "";
        foreach (string sphere in Spheres)
        {
            if (Spheres.Last() == sphere && Spheres.Count > 1)
            {
                spheres += " and ";
            }
            else if (spheres.Length > 0)
            {
                spheres += ", ";
            }

            spheres += sphere;
        }
        return spheres;
    }

    public string ToTreeLeafLink(DwarfObject pov = null)
    {
        string dead = DeathYear != -1 ? "<br/>" + HtmlStyleUtil.SymbolDead : "";
        return pov == null || pov != this
            ? "<a " + (Deity ? "class=\"hf_deity\"" : "") + " href=\"hf#" + Id + "\" title=\"" + Title + "\">" + GetRaceString() + "<br/>" + Name + dead + "</a>"
            : "<a " + (Deity ? "class=\"hf_deity\"" : "") + " title=\"" + Title + "\">" + GetRaceString() + "<br/>" + HtmlStyleUtil.CurrentDwarfObject(Name) + dead + "</a>";
    }

    public class Assignment : Position
    {
        public Assignment(Site site, int began, int ended, string title) : this(site?.CurrentOwner as Entity, began, ended, title)
        {
            Site = site;
        }

        public Assignment(Entity entity, int began, int ended, string title) : base(entity, began, ended, title)
        {
        }

        public Site Site { get; set; }

        public override string ToString()
        {
            if (Site != null)
            {
                return $"{Title} in  {Site.Name}";
            }
            if (Entity != null)
            {
                return $"{Title} of {Entity.Name}";
            }
            return Title;
        }
    }

    public class Position
    {
        [JsonIgnore]
        public Entity? Entity { get; set; }
        public string? EntityToLink => Entity?.ToLink(true);

        public string Title { get; set; }
        public int Began { get; set; }
        public int Ended { get; set; }
        public int Length { get; set; }

        public Position(Entity entity, int began, int ended, string title)
        {
            Entity = entity; Began = began; Ended = ended; Title = Formatting.InitCaps(title);
        }
    }

    public class State
    {
        public HfState HfState { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public State(HfState state, int start)
        {
            HfState = state;
            StartYear = start;
            EndYear = -1;
        }
    }

    public class CreatureType
    {
        public string Type { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int StartDay { get; set; }

        public CreatureType(string type, int startYear, int startMonth, int startDay)
        {
            Type = type;
            StartYear = startYear;
            StartMonth = startMonth;
            StartDay = startDay;
        }

        public CreatureType(string type, WorldEvent worldEvent) : this(type, worldEvent.Year, worldEvent.Month, worldEvent.Day)
        {
        }
    }

    public string CasteNoun(bool owner = false)
    {
        if (string.Equals(Caste, "male", StringComparison.OrdinalIgnoreCase))
        {
            return owner ? "his" : "he";
        }

        if (string.Equals(Caste, "female", StringComparison.OrdinalIgnoreCase))
        {
            return owner ? "her" : "she";
        }

        return owner ? "its" : "it";
    }

    public string GetRaceTitleString()
    {
        string hfraceString = "";

        if (Ghost)
        {
            hfraceString += "ghostly ";
        }

        if (Skeleton)
        {
            hfraceString += "skeletal ";
        }

        if (Zombie)
        {
            hfraceString += "zombie ";
        }

        if (string.Equals(Caste, "MALE", StringComparison.OrdinalIgnoreCase))
        {
            hfraceString += "male ";
        }
        else if (string.Equals(Caste, "FEMALE", StringComparison.OrdinalIgnoreCase))
        {
            hfraceString += "female ";
        }

        hfraceString += GetRaceString();

        return Formatting.AddArticle(hfraceString);
    }

    public string GetRaceString()
    {
        if (Race == null)
        {
            Race = CreatureInfo.Unknown;
        }
        if (Deity)
        {
            return Race.NameSingular.ToLower() + " deity";
        }

        if (Force)
        {
            return "force";
        }

        string raceString = "";
        if (!string.IsNullOrWhiteSpace(PreviousRace))
        {
            raceString += PreviousRace.ToLower() + " turned ";
        }
        else if (!string.IsNullOrWhiteSpace(AnimatedType) && !Name.Contains("Corpse"))
        {
            raceString += AnimatedType.ToLower();
        }
        else
        {
            raceString += Race.NameSingular.ToLower();
        }

        foreach (var creatureType in CreatureTypes)
        {
            raceString += " " + creatureType.Type;
        }

        return raceString;
    }

    private string GetRaceStringByWorldEvent(WorldEvent worldEvent)
    {
        return GetRaceStringForTimeStamp(worldEvent.Year, worldEvent.Month, worldEvent.Day);
    }

    private string GetRaceStringForTimeStamp(int year, int month, int day)
    {
        if (CreatureTypes.Count == 0)
        {
            return RaceString;
        }

        List<CreatureType> relevantCreatureTypes = GetRelevantCreatureTypesByTimeStamp(year, month, day);
        string raceString = "";
        if (!string.IsNullOrWhiteSpace(PreviousRace))
        {
            raceString += PreviousRace.ToLower();
        }
        else if (!string.IsNullOrWhiteSpace(AnimatedType))
        {
            raceString += AnimatedType.ToLower();
        }
        else
        {
            raceString += Race.NameSingular.ToLower();
        }

        foreach (var creatureType in relevantCreatureTypes)
        {
            raceString += " " + creatureType.Type;
        }

        return raceString;
    }

    private List<CreatureType> GetRelevantCreatureTypesByTimeStamp(int year, int month, int day)
    {
        List<CreatureType> relevantCreatureTypes = [];
        foreach (var creatureType in CreatureTypes)
        {
            if (creatureType.StartYear < year)
            {
                relevantCreatureTypes.Add(creatureType);
            }
            else if (creatureType.StartYear == year)
            {
                if (creatureType.StartMonth < month)
                {
                    relevantCreatureTypes.Add(creatureType);
                }
                else if (creatureType.StartMonth == month && creatureType.StartDay < day)
                {
                    relevantCreatureTypes.Add(creatureType);
                }
            }
        }
        return relevantCreatureTypes;
    }
}