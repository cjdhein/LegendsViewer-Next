using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Purge : EventCollection
{
    public string Ordinal;
    public string Adjective { get; set; }
    public Site Site;

    public Purge(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = string.Intern(property.Value); break;
                case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                case "adjective": Adjective = property.Value; break;
            }
        }
        Site.AddEventCollection(this);
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        return "a " + (!string.IsNullOrWhiteSpace(Adjective) ? Adjective.ToLower() + " " : "") + "purge";
    }
}
