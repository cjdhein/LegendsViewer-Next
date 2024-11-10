using LegendsViewer.Backend.Contracts;
using LegendsViewer.Backend.Extensions;
using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;
using System.Drawing;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class War : EventCollection, IHasComplexSubtype
{
    public int Length { get; set; }
    public int DeathCount { get; set; }
    public int AttackerDeathCount { get; set; }
    public int DefenderDeathCount { get; set; }

    public List<ListItemDto> MiscList
    {
        get
        {
            var list = new List<ListItemDto>();

            if (Attacker != null)
            {
                list.Add(new ListItemDto
                {
                    Title = $"Attacker",
                    Subtitle = $"{Attacker.ToLink(true, this)} (Deaths: {AttackerDeathCount} ✝)",
                });
            }

            if (Defender != null)
            {
                list.Add(new ListItemDto
                {
                    Title = $"Defender",
                    Subtitle = $"{Defender.ToLink(true, this)} (Deaths: {DefenderDeathCount} ✝)",
                });
            }
            return list;
        }
    }

    [JsonIgnore]
    public List<HistoricalFigure> NotableDeaths => AllEvents?.OfType<HfDied>().Where(death => death.HistoricalFigure != null).Select(death => death.HistoricalFigure!).ToList() ?? [];
    public List<string> NotableDeathLinks => NotableDeaths.ConvertAll(x => x.ToLink(true, this));
    public ChartDataDto DeathsByRace
    {
        get
        {
            Dictionary<CreatureInfo, int> deaths = [];

            foreach (var notableDeath in NotableDeaths)
            {
                if (deaths.TryGetValue(notableDeath.Race, out int deathCount))
                {
                    deaths[notableDeath.Race] = ++deathCount;
                }
                else
                {
                    deaths[notableDeath.Race] = 1;
                }
            }

            foreach (var squad in Battles.SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)))
            {
                if (deaths.TryGetValue(squad.Race, out int deathCount))
                {
                    deaths[squad.Race] = deathCount + squad.Deaths;
                }
                else
                {
                    deaths[squad.Race] = squad.Deaths;
                }
            }
            ChartDataDto deathsByRace = new();
            ChartDatasetDto deathsByRaceDataset = new();
            foreach (var death in deaths)
            {
                deathsByRace.Labels.Add(death.Key.NamePlural);
                deathsByRaceDataset.Data.Add(death.Value);
                if (World != null && World.MainRaces.TryGetValue(death.Key, out var raceColor))
                {
                    deathsByRaceDataset.BorderColor.Add(raceColor.ToRgbaString());
                    deathsByRaceDataset.BackgroundColor.Add(raceColor.ToRgbaString(0.2f));
                }
                else
                {
                    deathsByRaceDataset.BorderColor.Add(Color.SlateGray.ToRgbaString());
                    deathsByRaceDataset.BackgroundColor.Add(Color.SlateGray.ToRgbaString(0.2f));
                }
            }
            deathsByRace.Datasets.Add(deathsByRaceDataset);
            return deathsByRace;
        }
    }

    public List<DirectedChordDataDto> BattleDiagramData
    {
        get
        {
            var battleDiagramData = new List<DirectedChordDataDto>();
            if (Battles.Count > 0)
            {
                string defaultColor = Color.Gray.ToRgbaString(0.75f);
                foreach (var battle in Battles)
                {
                    battleDiagramData.Add(new DirectedChordDataDto
                    {
                        Source = battle.Attacker?.CurrentCiv?.Name ?? battle.Attacker?.Name ?? "Unknown",
                        Target = battle.Defender?.CurrentCiv?.Name ?? battle.Defender?.Name ?? "Unknown",
                        SourceColor = battle.Attacker?.LineColor.ToRgbaString(0.75f) ?? defaultColor,
                        TargetColor = battle.Defender?.LineColor.ToRgbaString(0.75f) ?? defaultColor,
                        Value = 100 / Battles.Count(b => b.Attacker?.CurrentCiv == battle.Attacker?.CurrentCiv),
                        Tooltip = $"{battle.Name} | {battle.Type} | Deaths: {battle.DeathCount}",
                        Href = $"/battle/{battle.Id}"
                    });
                }
            }
            return battleDiagramData;
        }
    }

    [JsonIgnore]
    public Entity? Attacker { get; set; }
    [JsonIgnore]
    public Entity? Defender { get; set; }
    [JsonIgnore]
    public List<Battle> Battles => EventCollections.OfType<Battle>().ToList();
    [JsonIgnore]
    public List<SiteConquered> Conquerings => EventCollections.OfType<SiteConquered>().ToList();
    [JsonIgnore]
    public List<Site?> SitesLost => Conquerings
        .Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction)
        .Select(conquering => conquering.Site)
        .ToList();
    [JsonIgnore]
    public List<Site?> AttackerSitesLost => DefenderConquerings
        .Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction)
        .Select(conquering => conquering.Site)
        .ToList();
    [JsonIgnore]
    public List<Site?> DefenderSitesLost => AttackerConquerings
        .Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction)
        .Select(conquering => conquering.Site)
        .ToList();
    [JsonIgnore]
    public List<EventCollection> AttackerVictories { get; set; } = [];
    [JsonIgnore]
    public List<EventCollection> DefenderVictories { get; set; } = [];
    [JsonIgnore]
    public List<Battle> AttackerBattleVictories => EventCollections.OfType<Battle>().Where(battle => battle.Victor?.EqualsOrParentEquals(Attacker) ?? false).ToList();
    [JsonIgnore]
    public List<Battle> DefenderBattleVictories => EventCollections.OfType<Battle>().Where(battle => battle.Victor?.EqualsOrParentEquals(Defender) ?? false).ToList();
    [JsonIgnore]
    public List<Battle> ErrorBattles => EventCollections.OfType<Battle>()
        .Where(battle =>
            (!battle.Attacker?.EqualsOrParentEquals(Attacker) ?? false) && (!battle.Attacker?.EqualsOrParentEquals(Defender) ?? false) ||
            (!battle.Defender?.EqualsOrParentEquals(Defender) ?? false) && (!battle.Defender?.EqualsOrParentEquals(Attacker) ?? false))
        .ToList();
    [JsonIgnore]
    public List<SiteConquered> AttackerConquerings => EventCollections.OfType<SiteConquered>().Where(conquering => conquering.Attacker?.EqualsOrParentEquals(Attacker) ?? false).ToList();
    [JsonIgnore]
    public List<SiteConquered> DefenderConquerings => EventCollections.OfType<SiteConquered>().Where(conquering => conquering.Attacker?.EqualsOrParentEquals(Defender) ?? false).ToList();
    public double AttackerToDefenderKills
    {
        get
        {
            if (AttackerDeathCount == 0 && DefenderDeathCount == 0)
            {
                return 0;
            }

            return AttackerDeathCount == 0 ? double.MaxValue : Math.Round(DefenderDeathCount / Convert.ToDouble(AttackerDeathCount), 2);
        }
    }
    public double AttackerToDefenderVictories
    {
        get
        {
            if (DefenderBattleVictories.Count == 0 && AttackerBattleVictories.Count == 0)
            {
                return 0;
            }

            return DefenderBattleVictories.Count == 0
                ? double.MaxValue
                : Math.Round(AttackerBattleVictories.Count / Convert.ToDouble(DefenderBattleVictories.Count), 2);
        }
    }

    public War(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "aggressor_ent_id": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                case "defender_ent_id": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
            }
        }

        Defender?.Wars.Add(this);
        Defender?.Parent?.Wars.Add(this);

        Attacker?.Wars.Add(this);
        Attacker?.Parent?.Wars.Add(this);

        if (EndYear >= 0)
        {
            Length = EndYear - StartYear;
        }
        else if (world.Events.Count > 0)
        {
            Length = world.Events.Last().Year - StartYear;
        }
        Attacker?.AddEventCollection(this);
        if (Defender != Attacker)
        {
            Defender?.AddEventCollection(this);
        }

        Icon = HtmlStyleUtil.GetIconString("sword-cross");
    }

    public void GenerateComplexSubType()
    {
        if (string.IsNullOrEmpty(Subtype))
        {
            Subtype = $"{Attacker?.ToLink(true, this)} => {Defender?.ToLink(true, this)}";
        }
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string title = GetTitle();

            return pov != this
                ? HtmlStyleUtil.GetAnchorString(Icon, "war", Id, title, Name)
                : HtmlStyleUtil.GetAnchorCurrentString(Icon, title, HtmlStyleUtil.CurrentDwarfObject(Name));
        }
        return Name;
    }

    public string GetTooltip()
    {
        string tooltip = Name;
        tooltip += "&#13";
        tooltip += GetTitle();
        return tooltip;
    }

    private string GetTitle()
    {
        string title = Type;
        title += "&#13";
        title += Attacker?.PrintEntity(false) + " (Attacker)";
        title += "&#13";
        title += Defender?.PrintEntity(false) + " (Defender)";
        title += "&#13";
        title += "Deaths: " + DeathCount + " | (" + StartYear + " - " + (EndYear == -1 ? "Present" : EndYear.ToString()) + ")";
        return title;
    }

    public override string ToString()
    {
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
