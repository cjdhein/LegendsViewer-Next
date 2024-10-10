using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class War : EventCollection, IHasComplexSubtype
{
    public int Length { get; set; }
    public int DeathCount { get; set; }
    private readonly Dictionary<CreatureInfo, int> _deaths = [];
    public Dictionary<CreatureInfo, int> Deaths
    {
        get
        {
            if (_deaths.Count > 0)
            {
                return _deaths;
            }
            foreach (Battle battle in Battles)
            {
                foreach (KeyValuePair<CreatureInfo, int> deathByRace in battle.Deaths)
                {
                    if (_deaths.ContainsKey(deathByRace.Key))
                    {
                        _deaths[deathByRace.Key] += deathByRace.Value;
                    }
                    else
                    {
                        _deaths[deathByRace.Key] = deathByRace.Value;
                    }
                }
            }
            return _deaths;
        }
    }
    public int AttackerDeathCount { get; set; }
    public int DefenderDeathCount { get; set; }
    public Entity? Attacker { get; set; }
    public Entity? Defender { get; set; }
    public List<Battle> Battles => EventCollections.OfType<Battle>().ToList();
    public List<SiteConquered> Conquerings => EventCollections.OfType<SiteConquered>().ToList();
    public List<Site?> SitesLost => Conquerings
        .Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction)
        .Select(conquering => conquering.Site)
        .ToList();
    public List<Site?> AttackerSitesLost => DefenderConquerings
        .Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction)
        .Select(conquering => conquering.Site)
        .ToList();
    public List<Site?> DefenderSitesLost => AttackerConquerings
        .Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction)
        .Select(conquering => conquering.Site)
        .ToList();
    public List<EventCollection> AttackerVictories { get; set; } = [];
    public List<EventCollection> DefenderVictories { get; set; } = [];
    public List<Battle> AttackerBattleVictories => EventCollections.OfType<Battle>().Where(battle => battle.Victor?.EqualsOrParentEquals(Attacker) ?? false).ToList();
    public List<Battle> DefenderBattleVictories => EventCollections.OfType<Battle>().Where(battle => battle.Victor?.EqualsOrParentEquals(Defender) ?? false).ToList();
    public List<Battle> ErrorBattles => EventCollections.OfType<Battle>()
        .Where(battle =>
            (!battle.Attacker?.EqualsOrParentEquals(Attacker) ?? false) && (!battle.Attacker?.EqualsOrParentEquals(Defender) ?? false) ||
            (!battle.Defender?.EqualsOrParentEquals(Defender) ?? false) && (!battle.Defender?.EqualsOrParentEquals(Attacker) ?? false))
        .ToList();
    public List<SiteConquered> AttackerConquerings => EventCollections.OfType<SiteConquered>().Where(conquering => conquering.Attacker?.EqualsOrParentEquals(Attacker) ?? false).ToList();
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
            string title = Type;
            title += "&#13";
            title += Attacker?.PrintEntity(false) + " (Attacker)";
            title += "&#13";
            title += Defender?.PrintEntity(false) + " (Defender)";
            title += "&#13";
            title += "Deaths: " + DeathCount + " | (" + StartYear + " - " + (EndYear == -1 ? "Present" : EndYear.ToString()) + ")";

            return pov != this
                ? HtmlStyleUtil.GetAnchorString(Icon, "war", Id, title, Name)
                : HtmlStyleUtil.GetAnchorCurrentString(Icon, title, HtmlStyleUtil.CurrentDwarfObject(Name));
        }
        return Name;
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
