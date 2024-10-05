using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Insurrection : EventCollection
{
    public static readonly string Icon = HtmlStyleUtil.GetIconString("map-marker-alert");
    public string Name { get; set; } = "";

    public Site? Site { get; set; }
    public Entity? TargetEntity { get; set; }
    public int Ordinal { get; set; }

    public List<HistoricalFigure> Deaths => GetSubEvents().OfType<HfDied>().Select(death => death.HistoricalFigure).ToList();
    public int DeathCount => Deaths.Count;

    public Insurrection(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "site_id":
                    Site = world.GetSite(Convert.ToInt32(property.Value));
                    break;
                case "target_enid":
                    TargetEntity = world.GetEntity(Convert.ToInt32(property.Value));
                    break;
                case "ordinal":
                    Ordinal = Convert.ToInt32(property.Value);
                    break;
            }
        }

        var insurrectionStart = Collection.OfType<InsurrectionStarted>().FirstOrDefault();
        if (insurrectionStart != null)
        {
            insurrectionStart.ActualStart = true;
        }
        TargetEntity?.AddEventCollection(this);
        Site?.AddEventCollection(this);

        Site?.Warfare.Add(this);

        Name = $"{Formatting.AddOrdinal(Ordinal)} insurrection";
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
