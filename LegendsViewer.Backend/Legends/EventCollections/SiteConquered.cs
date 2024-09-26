using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class SiteConquered : EventCollection
{
    public string Icon = "<i class=\"glyphicon fa-fw glyphicon-pawn\"></i>";

    public string Name { get => Formatting.AddOrdinal(Ordinal) + " " + ConquerType.GetDescription() + " of " + Site.Name; set { } }
    public int DeathCount { get => Deaths.Count; set { } }

    public int Ordinal { get; set; }
    public SiteConqueredType ConquerType { get; set; }
    public Site? Site { get; set; }
    public Entity? Attacker { get; set; }
    public Entity? Defender { get; set; }
    public Battle? Battle { get; set; }
    public List<HistoricalFigure> Deaths { get => GetSubEvents().OfType<HfDied>().Select(death => death.HistoricalFigure).ToList(); set { } }
    public SiteConquered()
    {
        Initialize();
    }

    public SiteConquered(List<Property> properties, World world)
        : base(properties, world)
    {
        Initialize();
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = Convert.ToInt32(property.Value); break;
                case "war_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                case "attacking_enid": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                case "defending_enid": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
            }
        }

        if (Collection.OfType<PlunderedSite>().Any())
        {
            ConquerType = SiteConqueredType.Pillaging;
        }
        else if (Collection.OfType<DestroyedSite>().Any())
        {
            ConquerType = SiteConqueredType.Destruction;
        }
        else if (Collection.OfType<NewSiteLeader>().Any() || Collection.OfType<SiteTakenOver>().Any())
        {
            ConquerType = SiteConqueredType.Conquest;
        }
        else
        {
            ConquerType = Collection.OfType<SiteTributeForced>().Any() ? SiteConqueredType.TributeEnforcement : SiteConqueredType.Invasion;
        }

        if (ConquerType == SiteConqueredType.Pillaging ||
            ConquerType == SiteConqueredType.Invasion ||
            ConquerType == SiteConqueredType.TributeEnforcement)
        {
            Notable = false;
        }

        Site.Warfare.Add(this);
        if (ParentCollection is War)
        {
            War war = ParentCollection as War;
            war.DeathCount += Collection.OfType<HfDied>().Count();

            if (Attacker == war.Attacker)
            {
                war.AttackerVictories.Add(this);
            }
            else
            {
                war.DefenderVictories.Add(this);
            }
        }
        Attacker.AddEventCollection(this);
        Defender.AddEventCollection(this);
        Site.AddEventCollection(this);
    }

    private void Initialize()
    {
        Ordinal = 1;
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        string name = "The ";
        name += Formatting.AddOrdinal(Ordinal) + " ";
        name += ConquerType.GetDescription() + " of " + Site.ToLink(false);
        if (link)
        {
            string title = Type;
            title += "&#13";
            if (Attacker != null)
            {
                title += Attacker.PrintEntity(false) + " (Attacker)(V)";
                title += "&#13";
            }
            if (Defender != null)
            {
                title += Defender.PrintEntity(false) + " (Defender)";
            }

            string linkedString = "";
            if (pov != this)
            {
                linkedString = "<a href = \"collection#" + Id + "\" title=\"" + title + "\"><font color=\"6E5007\">" + name + "</font></a>";
                if (pov != Battle)
                {
                    linkedString += " as a result of " + Battle.ToLink();
                }
            }
            else
            {
                linkedString = Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(name) + "</a>";
            }
            return linkedString;
        }
        return name;
    }
    public override string ToString()
    {
        return ToLink(false);
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
