using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Raid : EventCollection
{
    public static readonly string Icon = "<i class=\"fa fa-fw fa-bolt\"></i>";

    public string Name { get => "The " + Formatting.AddOrdinal(Ordinal) + " Raid of " + Site.Name; set { } }
    public int Ordinal { get; set; }
    public Location Coordinates { get; set; }
    public Site Site { get; set; }

    public WorldRegion Region
    {
        get
        {
            if (_region == null && Site?.Region != null)
            {
                _region = Site.Region;
            }
            return _region;
        }
        set => _region = value;
    }

    public UndergroundRegion UndergroundRegion { get; set; }
    public Entity Attacker { get; set; }
    public Entity Defender { get; set; }
    public int ItemsStolenCount { get => GetSubEvents().OfType<ItemStolen>().Count(); set { } }
    public EventCollection ParentEventCol { get; set; }

    private WorldRegion _region;

    public Raid(List<Property> properties, World world)
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
                case "attacking_enid": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                case "defending_enid": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
            }
        }
        Attacker.AddEventCollection(this);
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
        if (link)
        {
            string title = Type;
            title += "&#13";
            title += "Items Stolen: " + ItemsStolenCount;

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
