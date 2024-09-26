using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class War : EventCollection
{
    public static readonly string Icon = "<i class=\"glyphicon fa-fw glyphicon-queen\"></i>";

    public string Name { get; set; }
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
    public Entity Attacker { get; set; }
    public Entity Defender { get; set; }
    public List<Battle> Battles { get => Collections.OfType<Battle>().ToList(); set { } }
    public List<SiteConquered> Conquerings { get => Collections.OfType<SiteConquered>().ToList(); set { } }
    public List<Site> SitesLost { get => Conquerings.Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction).Select(conquering => conquering.Site).ToList(); set { } }
    public List<Site> AttackerSitesLost { get => DefenderConquerings.Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction).Select(conquering => conquering.Site).ToList(); set { } }
    public List<Site> DefenderSitesLost { get => AttackerConquerings.Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest || conquering.ConquerType == SiteConqueredType.Destruction).Select(conquering => conquering.Site).ToList(); set { } }
    public List<EventCollection> AttackerVictories { get; set; }
    public List<EventCollection> DefenderVictories { get; set; }
    public List<Battle> AttackerBattleVictories { get => Collections.OfType<Battle>().Where(battle => battle.Victor.EqualsOrParentEquals(Attacker)).ToList(); set { } }
    public List<Battle> DefenderBattleVictories { get => Collections.OfType<Battle>().Where(battle => battle.Victor.EqualsOrParentEquals(Defender)).ToList(); set { } }
    public List<Battle> ErrorBattles { get => Collections.OfType<Battle>().Where(battle => !battle.Attacker.EqualsOrParentEquals(Attacker) && !battle.Attacker.EqualsOrParentEquals(Defender) || !battle.Defender.EqualsOrParentEquals(Defender) && !battle.Defender.EqualsOrParentEquals(Attacker)).ToList(); set { } }
    public List<SiteConquered> AttackerConquerings { get => Collections.OfType<SiteConquered>().Where(conquering => conquering.Attacker.EqualsOrParentEquals(Attacker)).ToList(); set { } }
    public List<SiteConquered> DefenderConquerings { get => Collections.OfType<SiteConquered>().Where(conquering => conquering.Attacker.EqualsOrParentEquals(Defender)).ToList(); set { } }
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
        set { }
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
        set { }
    }

    public War()
    {
        Initialize();
    }

    public War(List<Property> properties, World world)
        : base(properties, world)
    {
        Initialize();
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "aggressor_ent_id": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                case "defender_ent_id": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
            }
        }

        Defender.Wars.Add(this);
        Defender.Parent?.Wars.Add(this);

        Attacker.Wars.Add(this);
        Attacker.Parent?.Wars.Add(this);

        if (EndYear >= 0)
        {
            Length = EndYear - StartYear;
        }
        else if (world.Events.Count > 0)
        {
            Length = world.Events.Last().Year - StartYear;
        }
        Attacker.AddEventCollection(this);
        if (Defender != Attacker)
        {
            Defender.AddEventCollection(this);
        }
    }

    private void Initialize()
    {
        Name = "";
        Length = 0;
        DeathCount = 0;
        AttackerDeathCount = 0;
        DefenderDeathCount = 0;
        Attacker = null;
        Defender = null;
        AttackerVictories = [];
        DefenderVictories = [];
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        if (link)
        {
            string title = Type;
            title += "&#13";
            title += Attacker.PrintEntity(false) + " (Attacker)";
            title += "&#13";
            title += Defender.PrintEntity(false) + " (Defender)";
            title += "&#13";
            title += "Deaths: " + DeathCount + " | (" + StartYear + " - " + (EndYear == -1 ? "Present" : EndYear.ToString()) + ")";

            string linkedString = pov != this
                ? Icon + "<a href = \"collection#" + Id + "\" title=\"" + title + "\"><font color=\"#6E5007\">" + Name + "</font></a>"
                : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
            return linkedString;
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
