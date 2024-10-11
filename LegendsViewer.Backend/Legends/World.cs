using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldLinks;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends;

public class World : IDisposable, IWorld
{
    public Dictionary<CreatureInfo, Color> MainRaces { get; } = [];

    public string Name { get; set; } = string.Empty;
    public string AlternativeName { get; set; } = string.Empty;

    public List<WorldRegion> Regions { get; } = [];
    public List<UndergroundRegion> UndergroundRegions { get; } = [];
    public List<Landmass> Landmasses { get; } = [];
    public List<MountainPeak> MountainPeaks { get; } = [];
    public List<Identity> Identities { get; } = [];
    public List<River> Rivers { get; } = [];
    public List<Site> Sites { get; } = [];
    public List<HistoricalFigure> HistoricalFigures { get; } = [];
    public List<Entity> Entities { get; } = [];
    public List<Era> Eras { get; } = [];
    public List<Artifact> Artifacts { get; } = [];
    public List<WorldConstruction> WorldConstructions { get; } = [];
    public List<PoeticForm> PoeticForms { get; } = [];
    public List<MusicalForm> MusicalForms { get; } = [];
    public List<DanceForm> DanceForms { get; } = [];
    public List<WrittenContent> WrittenContents { get; } = [];
    public List<Structure> Structures { get; } = [];

    public List<EventCollection> EventCollections { get; } = [];
    public List<War> Wars => EventCollections.OfType<War>().ToList();
    public List<Battle> Battles => EventCollections.OfType<Battle>().ToList();
    public List<Duel> Duels => EventCollections.OfType<Duel>().ToList();
    public List<Raid> Raids => EventCollections.OfType<Raid>().ToList();
    public List<SiteConquered> SiteConquerings => EventCollections.OfType<SiteConquered>().ToList();

    public List<Insurrection> Insurrections => EventCollections.OfType<Insurrection>().ToList();
    public List<Persecution> Persecutions => EventCollections.OfType<Persecution>().ToList();
    public List<Purge> Purges => EventCollections.OfType<Purge>().ToList();
    public List<EntityOverthrownCollection> Coups => EventCollections.OfType<EntityOverthrownCollection>().ToList();

    public List<BeastAttack> BeastAttacks => EventCollections.OfType<BeastAttack>().ToList();
    public List<Abduction> Abductions => EventCollections.OfType<Abduction>().ToList();
    public List<Theft> Thefts => EventCollections.OfType<Theft>().ToList();

    public List<ProcessionCollection> Processions => EventCollections.OfType<ProcessionCollection>().ToList();
    public List<PerformanceCollection> Performances => EventCollections.OfType<PerformanceCollection>().ToList();
    public List<Journey> Journeys => EventCollections.OfType<Journey>().ToList();
    public List<CompetitionCollection> Competitions => EventCollections.OfType<CompetitionCollection>().ToList();
    public List<CeremonyCollection> Ceremonies => EventCollections.OfType<CeremonyCollection>().ToList();
    public List<Occasion> Occasions => EventCollections.OfType<Occasion>().ToList();

    public List<EntityPopulation> EntityPopulations { get; } = [];
    public List<Population> SitePopulations { get; } = [];
    public List<Population> CivilizedPopulations { get; } = [];
    public List<Population> OutdoorPopulations { get; } = [];
    public List<Population> UndergroundPopulations { get; } = [];

    public List<WorldEvent> Events { get; } = [];

    public int Width { get; set; }
    public int Height { get; set; }
    public int CurrentYear { get; set; }
    public int CurrentMonth { get; set; }
    public int CurrentDay { get; set; }

    public List<WorldObject> PlayerRelatedObjects { get; } = [];

    public readonly Dictionary<int, WorldEvent> SpecialEventsById = [];
    private readonly List<CreatureInfo> _creatureInfos = [];
    private readonly Dictionary<string, CreatureInfo> _creatureInfosById = [];

    public readonly Dictionary<string, List<HistoricalFigure>> Breeds = [];

    public StringBuilder Log { get; } = new StringBuilder();
    public ParsingErrors ParsingErrors { get; } = new ParsingErrors();

    public readonly List<Era> TempEras = [];
    public bool FilterBattles = true;

    private readonly List<HistoricalFigure> _hFtoHfLinkHFs = [];
    private readonly List<Property> _hFtoHfLinks = [];

    private readonly List<HistoricalFigure> _hFtoEntityLinkHFs = [];
    private readonly List<Property> _hFtoEntityLinks = [];

    private readonly List<HistoricalFigure> _hFtoSiteLinkHFs = [];
    private readonly List<Property> _hFtoSiteLinks = [];

    private readonly List<HistoricalFigure> _reputationHFs = [];
    private readonly List<Property> _reputations = [];

    private readonly List<Entity> _entityEntityLinkEntities = [];// legends_plus.xml
    private readonly List<Property> _entityEntityLinks = [];// legends_plus.xml

    public World()
    {
        MainRaces.Clear();
        Log.Clear();
        ParsingErrors.Clear();
    }

    public async Task ParseAsync(string xmlFile, string? xmlPlusFile, string? historyFile, string? sitesAndPopulationsFile, string? mapFile)
    {
        Stopwatch sw = new();
        sw.Start();

        XmlParser xmlParser = new(this, xmlFile, xmlPlusFile);
        await xmlParser.ParseAsync();
        if (!string.IsNullOrWhiteSpace(historyFile))
        {
            HistoryParser history = new(this, historyFile);
            Log.Append(history.Parse());
        }
        if (!string.IsNullOrWhiteSpace(sitesAndPopulationsFile))
        {
            SitesAndPopulationsParser sitesAndPopulations = new(this, sitesAndPopulationsFile);
            sitesAndPopulations.Parse();
        }

        ProcessHFtoEntityLinks();
        ResolveEntityToEntityPopulation();
        ResolveHfToEntityPopulation();
        ResolveStructureProperties();
        ResolveSitePropertyOwners();
        ResolveHonorEntities();
        ResolveMountainPeakToRegionLinks();
        ResolveSiteToRegionLinks();
        ResolveRegionProperties();
        ResolveArtifactProperties();
        ResolveArtformEventsProperties();
        ResolveEntityIsMainCiv();

        GenerateCivColors();

        Log.AppendLine(ParsingErrors.Print());

        sw.Stop();
        var minutes = sw.Elapsed.Minutes;
        var seconds = sw.Elapsed.Seconds;
        var milliSeconds = sw.Elapsed.Milliseconds;
        Log.Append("Duration:");
        if (minutes > 0)
        {
            Log.Append(' ').Append(minutes).Append(" mins,");
        }
        Log.Append(' ')
            .Append(seconds)
            .Append(" secs,")
            .Append(' ')
            .AppendFormat("{0:D3}", milliSeconds)
            .AppendLine(" ms");
    }

    private void ResolveEntityIsMainCiv()
    {
        foreach (var entity in Entities.Where(e => e.EntityType == EntityType.Civilization))
        {
            if (!entity.IsCiv && !string.IsNullOrWhiteSpace(entity.Name))
            {
                entity.IsCiv = true;
            }
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? entity.GetTitle() : entity.Name;
        }
    }

    public void AddPlayerRelatedDwarfObjects(WorldObject dwarfObject)
    {
        if (dwarfObject == null)
        {
            return;
        }
        if (PlayerRelatedObjects.Contains(dwarfObject))
        {
            return;
        }
        PlayerRelatedObjects.Add(dwarfObject);
    }

    private void GenerateCivColors()
    {
        List<Entity> civs = Entities.Where(entity => entity.IsCiv).ToList();
        List<CreatureInfo> races = civs.GroupBy(entity => entity.Race).Select(entity => entity.Key).OrderBy(creatureInfo => creatureInfo.NamePlural).ToList();

        //Calculates color
        //Creates a variety of colors
        //Races 1 to 6 get a medium color
        //Races 7 to 12 get a light color
        //Races 13 to 18 get a dark color
        //19+ reduced color variance
        const int maxHue = 300;
        int colorVariance;
        if (races.Count <= 1)
        {
            colorVariance = 0;
        }
        else if (races.Count <= 6)
        {
            colorVariance = Convert.ToInt32(Math.Floor(maxHue / Convert.ToDouble(races.Count - 1)));
        }
        else
        {
            colorVariance = races.Count > 18 ? Convert.ToInt32(Math.Floor(maxHue / (Math.Ceiling(races.Count / 3.0) - 1))) : 60;
        }

        foreach (Entity civ in civs)
        {
            int colorIndex = races.IndexOf(civ.Race);
            Color raceColor;
            if (colorIndex * colorVariance < 360)
            {
                raceColor = Formatting.HsvToColor(colorIndex * colorVariance, 1, 1.0);
            }
            else if (colorIndex * colorVariance < 720)
            {
                raceColor = Formatting.HsvToColor(colorIndex * colorVariance - 360, 0.4, 1);
            }
            else
            {
                raceColor = colorIndex * colorVariance < 1080 ? Formatting.HsvToColor(colorIndex * colorVariance - 720, 1, 0.4) : Color.Black;
            }

            const int alpha = 176;

            MainRaces.TryAdd(civ.Race, raceColor);

            civ.LineColor = Color.FromArgb(alpha, raceColor);

            foreach (var childGroup in civ.Groups)
            {
                childGroup.LineColor = civ.LineColor;
            }
        }

        // generate Subtypes that need color infos
        foreach (var collection in EventCollections.OfType<IHasComplexSubtype>())
        {
            collection.GenerateComplexSubType();
        }
    }

    #region GetWorldItemsFunctions

    public WorldRegion? GetRegion(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < Regions.Count && Regions[id].Id == id ? Regions[id] : Regions.GetWorldObject(id);
    }
    public UndergroundRegion? GetUndergroundRegion(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < UndergroundRegions.Count && UndergroundRegions[id].Id == id
            ? UndergroundRegions[id]
            : UndergroundRegions.GetWorldObject(id);
    }
    public HistoricalFigure? GetHistoricalFigure(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < HistoricalFigures.Count && HistoricalFigures[id].Id == id
            ? HistoricalFigures[id]
            : HistoricalFigures.GetWorldObject(id) ?? HistoricalFigure.Unknown;
    }
    public Entity? GetEntity(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < Entities.Count && Entities[id].Id == id ? Entities[id] : Entities.GetWorldObject(id);
    }

    public Artifact? GetArtifact(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < Artifacts.Count && Artifacts[id].Id == id ? Artifacts[id] : Artifacts.GetWorldObject(id);
    }
    public PoeticForm? GetPoeticForm(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < PoeticForms.Count && PoeticForms[id].Id == id ? PoeticForms[id] : PoeticForms.GetWorldObject(id);
    }
    public MusicalForm? GetMusicalForm(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < MusicalForms.Count && MusicalForms[id].Id == id ? MusicalForms[id] : MusicalForms.GetWorldObject(id);
    }
    public DanceForm? GetDanceForm(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < DanceForms.Count && DanceForms[id].Id == id ? DanceForms[id] : DanceForms.GetWorldObject(id);
    }
    public WrittenContent? GetWrittenContent(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < WrittenContents.Count && WrittenContents[id].Id == id ? WrittenContents[id] : WrittenContents.GetWorldObject(id);
    }
    public Landmass? GetLandmass(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < Landmasses.Count && Landmasses[id].Id == id ? Landmasses[id] : Landmasses.GetWorldObject(id);
    }
    public River? GetRiver(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < Rivers.Count && Rivers[id].Id == id ? Rivers[id] : Rivers.GetWorldObject(id);
    }
    public MountainPeak? GetMountainPeak(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < MountainPeaks.Count && MountainPeaks[id].Id == id ? MountainPeaks[id] : MountainPeaks.GetWorldObject(id);
    }

    public EntityPopulation? GetEntityPopulation(int id)
    {
        if (id < 0)
        {
            return null;
        }

        return id < EntityPopulations.Count && EntityPopulations[id].Id == id ? EntityPopulations[id] : EntityPopulations.GetWorldObject(id);
    }

    public T? GetEventCollection<T>(int id) where T : EventCollection => GetEventCollection(id) as T;

    public EventCollection? GetEventCollection(int id)
    {
        if (id < 0)
        {
            return null;
        }

        if (id < EventCollections.Count && EventCollections[id].Id == id)
        {
            return EventCollections[id];
        }
        int min = 0;
        int max = EventCollections.Count - 1;
        while (min <= max)
        {
            int mid = min + (max - min) / 2;
            if (id > EventCollections[mid].Id)
            {
                min = mid + 1;
            }
            else if (id < EventCollections[mid].Id)
            {
                max = mid - 1;
            }
            else
            {
                return EventCollections[mid];
            }
        }
        return null;
    }

    public WorldEvent? GetEvent(int id)
    {
        if (id < 0)
        {
            return null;
        }

        if (id < Events.Count && Events[id].Id == id)
        {
            return Events[id];
        }
        int min = 0;
        int max = Events.Count - 1;
        while (min <= max)
        {
            int mid = min + (max - min) / 2;
            if (id > Events[mid].Id)
            {
                min = mid + 1;
            }
            else if (id < Events[mid].Id)
            {
                max = mid - 1;
            }
            else
            {
                return Events[mid];
            }
        }
        return null;
    }

    public Structure? GetStructure(int id)
    {
        if (id < 0)
        {
            return null;
        }

        if (id < Structures.Count && Structures[id].Id == id)
        {
            return Structures[id];
        }
        int min = 0;
        int max = Structures.Count - 1;
        while (min <= max)
        {
            int mid = min + (max - min) / 2;
            if (id > Structures[mid].Id)
            {
                min = mid + 1;
            }
            else if (id < Structures[mid].Id)
            {
                max = mid - 1;
            }
            else
            {
                return Structures[mid];
            }
        }
        return null;
    }

    public Site? GetSite(int id)
    {
        // Sites start with id = 1 in xml instead of 0 like every other object
        if (id <= 0)
        {
            return null;
        }

        return id <= Sites.Count && Sites[id - 1].Id == id ? Sites[id - 1] : Sites.GetWorldObject(id);
    }

    public WorldConstruction? GetWorldConstruction(int id)
    {
        // WorldConstructions start with id = 1 in xml instead of 0 like every other object
        if (id <= 0)
        {
            return null;
        }

        return id <= WorldConstructions.Count && WorldConstructions[id - 1].Id == id
            ? WorldConstructions[id - 1]
            : WorldConstructions.GetWorldObject(id);
    }

    public Era? GetEra(int id)
    {
        return Eras.Find(era => era.Id == id);
    }

    public CreatureInfo GetCreatureInfo(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return CreatureInfo.Unknown;
        }

        if (_creatureInfosById.ContainsKey(id.ToLower()))
        {
            return _creatureInfosById[id.ToLower()];
        }
        var creatureInfo = _creatureInfos.Find(ci =>
            ci.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase) ||
            ci.NameSingular.Equals(id, StringComparison.InvariantCultureIgnoreCase) ||
            ci.NamePlural.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        return creatureInfo ?? AddCreatureInfo(id);
    }

    private CreatureInfo AddCreatureInfo(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return CreatureInfo.Unknown;
        }
        CreatureInfo creatureInfo = new(id);
        _creatureInfos.Add(creatureInfo);
        _creatureInfosById[id.ToLower()] = creatureInfo;
        return creatureInfo;
    }

    public void AddCreatureInfo(CreatureInfo creatureInfo)
    {
        _creatureInfos.Add(creatureInfo);
        _creatureInfosById[creatureInfo.Id.ToLower()] = creatureInfo;
    }

    #endregion GetWorldItemsFunctions

    #region AfterXMLSectionProcessing

    public void AddHFtoHfLink(HistoricalFigure hf, Property link)
    {
        _hFtoHfLinkHFs.Add(hf);
        _hFtoHfLinks.Add(link);
    }

    public void ProcessHFtoHfLinks()
    {
        for (int i = 0; i < _hFtoHfLinks.Count; i++)
        {
            Property link = _hFtoHfLinks[i];
            HistoricalFigure hf = _hFtoHfLinkHFs[i];
            HistoricalFigureLink relation = new(link.SubProperties, this);
            hf.RelatedHistoricalFigures.Add(relation);
        }

        _hFtoHfLinkHFs.Clear();
        _hFtoHfLinks.Clear();
    }

    public void AddHFtoEntityLink(HistoricalFigure hf, Property link)
    {
        _hFtoEntityLinkHFs.Add(hf);
        _hFtoEntityLinks.Add(link);
    }

    public void ProcessHFtoEntityLinks()
    {
        for (int i = 0; i < _hFtoEntityLinks.Count; i++)
        {
            Property link = _hFtoEntityLinks[i];
            HistoricalFigure hf = _hFtoEntityLinkHFs[i];
            EntityLink relatedEntity = new(link.SubProperties, this);
            if (relatedEntity.Entity != null)
            {
                if (relatedEntity.Type != EntityLinkType.Enemy || relatedEntity.Type == EntityLinkType.Enemy && relatedEntity.Entity.IsCiv)
                {
                    hf.RelatedEntities.Add(relatedEntity);
                }
            }
        }

        _hFtoEntityLinkHFs.Clear();
        _hFtoEntityLinks.Clear();
    }

    public void AddHFtoSiteLink(HistoricalFigure hf, Property link)
    {
        _hFtoSiteLinkHFs.Add(hf);
        _hFtoSiteLinks.Add(link);
    }

    public void ProcessHFtoSiteLinks()
    {
        for (int i = 0; i < _hFtoSiteLinks.Count; i++)
        {
            Property link = _hFtoSiteLinks[i];
            HistoricalFigure hf = _hFtoSiteLinkHFs[i];
            SiteLink hfToSiteLink = new(link.SubProperties, this);
            hf.RelatedSites.Add(hfToSiteLink);
            hfToSiteLink.Site?.RelatedHistoricalFigures.Add(hf);
        }

        _hFtoSiteLinkHFs.Clear();
        _hFtoSiteLinks.Clear();
    }

    public void AddEntityEntityLink(Entity entity, Property property)
    {
        _entityEntityLinkEntities.Add(entity);
        _entityEntityLinks.Add(property);
    }

    public void ProcessEntityEntityLinks()
    {
        for (int i = 0; i < _entityEntityLinkEntities.Count; i++)
        {
            Entity entity = _entityEntityLinkEntities[i];
            Property entityLink = _entityEntityLinks[i];
            entityLink.Known = true;
            var entityEntityLink = new EntityEntityLink(entityLink.SubProperties, this);
            entity.EntityEntityLinks.Add(entityEntityLink);
        }
    }

    public void AddReputation(HistoricalFigure hf, Property link)
    {
        _reputationHFs.Add(hf);
        _reputations.Add(link);
    }

    public void ProcessReputations()
    {
        for (int i = 0; i < _reputations.Count; i++)
        {
            Property reputation = _reputations[i];
            HistoricalFigure hf = _reputationHFs[i];
            EntityReputation entityReputation = new(reputation.SubProperties, this);
            hf.Reputations.Add(entityReputation);
        }

        _reputationHFs.Clear();
        _reputations.Clear();
    }

    private void ResolveStructureProperties()
    {
        foreach (Structure structure in Structures)
        {
            structure.Resolve(this);
        }
    }

    private void ResolveSitePropertyOwners()
    {
        foreach (var site in Sites)
        {
            if (site.SiteProperties.Count > 0)
            {
                foreach (SiteProperty siteProperty in site.SiteProperties)
                {
                    siteProperty.Resolve(this);
                }
            }
        }
    }

    private void ResolveHonorEntities()
    {
        foreach (var historicalFigure in HistoricalFigures.Where(hf => hf.HonorEntity != null))
        {
            historicalFigure.HonorEntity.Resolve(this, historicalFigure);
        }
    }

    private void ResolveArtifactProperties()
    {
        foreach (var artifact in Artifacts)
        {
            artifact.Resolve(this);
        }
    }

    private void ResolveRegionProperties()
    {
        foreach (var region in Regions)
        {
            region.Resolve(this);
        }
    }

    private void ResolveArtformEventsProperties()
    {
        foreach (var formCreated in Events.OfType<DanceFormCreated>())
        {
            if (int.TryParse(formCreated.FormId, out var id))
            {
                formCreated.ArtForm = GetDanceForm(id);
                formCreated.ArtForm?.AddEvent(formCreated);
            }
        }
        foreach (var formCreated in Events.OfType<MusicalFormCreated>())
        {
            if (int.TryParse(formCreated.FormId, out var id))
            {
                formCreated.ArtForm = GetMusicalForm(id);
                formCreated.ArtForm?.AddEvent(formCreated);
            }
        }
        foreach (var formCreated in Events.OfType<PoeticFormCreated>())
        {
            if (int.TryParse(formCreated.FormId, out var id))
            {
                formCreated.ArtForm = GetPoeticForm(id);
                formCreated.ArtForm?.AddEvent(formCreated);
            }
        }
        foreach (var occasionEvent in Events.OfType<OccasionEvent>())
        {
            occasionEvent.ResolveArtForm();
        }
        foreach (var writtenContentComposed in Events.OfType<WrittenContentComposed>())
        {
            if (int.TryParse(writtenContentComposed.WrittenContentId, out var id))
            {
                writtenContentComposed.WrittenContent = GetWrittenContent(id);
                writtenContentComposed.WrittenContent?.AddEvent(writtenContentComposed);
            }
        }
    }

    private void ResolveMountainPeakToRegionLinks()
    {
        foreach (MountainPeak peak in MountainPeaks)
        {
            foreach (WorldRegion region in Regions)
            {
                if (region.Coordinates.Contains(peak.Coordinates[0]))
                {
                    peak.Region = region;
                    region.MountainPeaks.Add(peak);
                    break;
                }
            }
        }
    }

    private void ResolveSiteToRegionLinks()
    {
        foreach (Site site in Sites)
        {
            foreach (WorldRegion region in Regions)
            {
                if (region.Coordinates.Intersect(site.Coordinates).Any())
                {
                    site.Region = region;
                    site.Subtype = region.RegionType.GetDescription();

                    region.Sites.Add(site);
                    break;
                }
            }
        }
    }

    private void ResolveEntityToEntityPopulation()
    {
        foreach (var entityPopulation in EntityPopulations)
        {
            Entity? civ = GetEntity(entityPopulation.EntityId);
            if (civ != null)
            {
                civ.EntityPopulation = entityPopulation;
                entityPopulation.Entity = civ;
            }
        }
    }

    private void ResolveHfToEntityPopulation()
    {
        foreach (HistoricalFigure historicalFigure in HistoricalFigures.Where(hf => hf.EntityPopulationId != -1))
        {
            historicalFigure.EntityPopulation = GetEntityPopulation(historicalFigure.EntityPopulationId);
            if (historicalFigure.EntityPopulation != null)
            {
                historicalFigure.EntityPopulation.Members.Add(historicalFigure);
            }
        }
    }

    #endregion AfterXMLSectionProcessing

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            Clear();
        }
    }

    public void Clear()
    {
        Name = string.Empty;
        AlternativeName = string.Empty;

        Regions.Clear();
        UndergroundRegions.Clear();
        Landmasses.Clear();
        MountainPeaks.Clear();
        Identities.Clear();
        Rivers.Clear();
        Sites.Clear();
        HistoricalFigures.Clear();
        Entities.Clear();
        Wars.Clear();
        Battles.Clear();
        BeastAttacks.Clear();
        Eras.Clear();
        Artifacts.Clear();
        WorldConstructions.Clear();
        PoeticForms.Clear();
        MusicalForms.Clear();
        DanceForms.Clear();
        WrittenContents.Clear();
        Structures.Clear();
        Events.Clear();
        EventCollections.Clear();
        EntityPopulations.Clear();
        SitePopulations.Clear();
        CivilizedPopulations.Clear();
        OutdoorPopulations.Clear();
        UndergroundPopulations.Clear();

        Width = 0;
        Height = 0;
        PlayerRelatedObjects.Clear();

        SpecialEventsById.Clear();
        _creatureInfos.Clear();
        _creatureInfosById.Clear();

        Breeds.Clear();

        TempEras.Clear();
        FilterBattles = true;

        _hFtoHfLinkHFs.Clear();
        _hFtoHfLinks.Clear();

        _hFtoEntityLinkHFs.Clear();
        _hFtoEntityLinks.Clear();

        _hFtoSiteLinkHFs.Clear();
        _hFtoSiteLinks.Clear();

        _reputationHFs.Clear();
        _reputations.Clear();

        _entityEntityLinkEntities.Clear();
        _entityEntityLinks.Clear();

        MainRaces.Clear();
        Log.Clear();
        ParsingErrors.Clear();
    }
}
