using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Raid : EventCollection
{
    public static readonly string Icon = HtmlStyleUtil.GetIconString("lightning-bolt");
    public string Name { get; set; } = "";

    public int Ordinal { get; set; }
    public Location? Coordinates { get; set; }
    public Site? Site { get; set; }

    public WorldRegion? Region
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

    public UndergroundRegion? UndergroundRegion { get; set; }
    public Entity? Attacker { get; set; }
    public Entity? Defender { get; set; }
    public int ItemsStolenCount => GetSubEvents().OfType<ItemStolen>().Count();
    public List<HistoricalFigure> Deaths => GetSubEvents().OfType<HfDied>().Select(death => death.HistoricalFigure).ToList();
    public int DeathCount => Deaths.Count;
    public EventCollection? ParentEventCol { get; set; }

    private WorldRegion? _region;

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
        Attacker?.AddEventCollection(this);
        Defender?.AddEventCollection(this);
        Region?.AddEventCollection(this);
        UndergroundRegion?.AddEventCollection(this);
        Site?.AddEventCollection(this);

        Site?.Warfare.Add(this);

        Name = $"{Formatting.AddOrdinal(Ordinal)} raid";
    }

    private void Initialize()
    {
        Ordinal = 1;
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string title = GetTitle();
            string linkedString = "the ";
            linkedString += pov != this
                ? HtmlStyleUtil.GetAnchorString(Icon, "collection", Id, title, Name)
                : HtmlStyleUtil.GetAnchorCurrentString(Icon, title, HtmlStyleUtil.CurrentDwarfObject(Name));

            if (Site != null && pov != Site)
            {
                linkedString += $" in {Site.ToLink(true, this)}";
            }
            return linkedString;
        }
        return ToString();
    }

    private string GetTitle()
    {
        string title = Type;
        title += "&#13";
        title += "Items Stolen: " + ItemsStolenCount;
        title += "&#13";
        title += "Site: ";
        title += Site != null ? Site.ToLink(false) : "UNKNOWN";
        return title;
    }

    public override string ToString()
    {
        return $"the {Name} in {Site}";
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
