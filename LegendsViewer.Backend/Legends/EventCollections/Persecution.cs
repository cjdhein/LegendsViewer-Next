using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Persecution : EventCollection
{
    public int Ordinal { get; set; } = -1;
    public Location? Coordinates;

    public WorldRegion? Region;
    public UndergroundRegion? UndergroundRegion;
    public Site? Site;
    public Entity? TargetEntity;

    public List<HistoricalFigure> Deaths => GetSubEvents().OfType<HfDied>().Select(death => death.HistoricalFigure).ToList();
    public int DeathCount => Deaths.Count;

    public Persecution(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = Convert.ToInt32(property.Value); break;
                case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                case "parent_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                case "target_entity_id": TargetEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
            }
        }

        Region?.AddEventCollection(this);
        UndergroundRegion?.AddEventCollection(this);
        Site?.AddEventCollection(this);
        TargetEntity?.AddEventCollection(this);

        Site?.Warfare.Add(this);

        Name = $"{Formatting.AddOrdinal(Ordinal)} persecution";

        Icon = HtmlStyleUtil.GetIconString("map-marker-alert");
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

            if (TargetEntity != null && pov != TargetEntity)
            {
                linkedString += $" of {TargetEntity.ToLink(true, this)}";
            }

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
        title += "Of Entity: ";
        title += TargetEntity != null ? TargetEntity.ToLink(false) : "UNKNOWN";
        title += "&#13";
        title += "Site: ";
        title += Site != null ? Site.ToLink(false) : "UNKNOWN";
        return title;
    }

    public override string ToString()
    {
        return $"the {Name} in {Site}";
    }
}
