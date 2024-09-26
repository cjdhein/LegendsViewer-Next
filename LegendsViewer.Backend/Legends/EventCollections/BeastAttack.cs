using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class BeastAttack : EventCollection
{
    private const string Icon = "<i class=\"glyphicon fa-fw glyphicon-knight\"></i>";

    public string Name { get => Formatting.AddOrdinal(Ordinal) + " Rampage of " + (Beast != null ? Beast.Name : "an unknown creature"); set { } }
    public int DeathCount { get => Deaths.Count; set { } }

    public int Ordinal { get; set; }
    public Location Coordinates { get; set; }
    public WorldRegion Region { get; set; }
    public UndergroundRegion UndergroundRegion { get; set; }
    public Site Site { get; set; }
    public Entity Defender { get; set; }

    private HistoricalFigure? _beast;
    public HistoricalFigure? Beast
    {
        get => _beast;
        set
        {
            _beast = value;
            _beast?.AddEventCollection(this);
        }
    }

    public List<HistoricalFigure> Deaths { get => GetSubEvents().OfType<HfDied>().Select(death => death.HistoricalFigure).ToList(); set { } }

    // BUG in XML? 
    // ParentCollection was never set prior to DF 0.42.xx and is now often set to an occasion
    // but DF legends mode does not show it.
    // http://www.bay12forums.com/smf/index.php?topic=154617.msg6669851#msg6669851
    public EventCollection ParentEventCol { get; set; }

    public BeastAttack(List<Property> properties, World world)
        : base(properties, world)
    {
        Initialize();

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = Convert.ToInt32(property.Value); break;
                case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                case "parent_eventcol": ParentEventCol = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                case "defending_enid": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
            }
        }

        Site.BeastAttacks.Add(this);

        //--------Attacking Beast is calculated after parsing event collections in ParseXML()
        //--------So that it can also look at eventsList from duel sub collections to calculate the Beast

        //-------Fill in some missing event details with details from collection
        //-------Filled in after parsing event collections in ParseXML()
        Defender.AddEventCollection(this);
        Region.AddEventCollection(this);
        UndergroundRegion.AddEventCollection(this);
        Site.AddEventCollection(this);
    }

    private void Initialize()
    {
        Ordinal = 1;
        Coordinates = new Location(0, 0);
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        string name = "";
        name = "The " + Formatting.AddOrdinal(Ordinal) + " rampage of ";
        if (Beast != null && pov == Beast)
        {
            name += Beast.ToLink(false, Beast);
        }
        else if (Beast != null)
        {
            name += Beast.Name;
        }
        else
        {
            name += "an unknown creature";
        }

        if (pov != Site)
        {
            name += " in " + Site.ToLink(false);
        }

        if (link)
        {
            string title = "Beast Attack";
            title += "&#13";
            title += "Events: " + GetSubEvents().Count;

            string linkedString = pov != this
                ? Icon + "<a href = \"collection#" + Id + "\" title=\"" + title + "\"><font color=\"#6E5007\">" + name + "</font></a>"
                : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(name) + "</a>";
            return linkedString;
        }
        return pov == this ? "Rampage of " + Beast.ToLink(false, Beast) : name;
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
