using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.EventCollections;

internal class Insurrection : EventCollection
{
    public string Name { get; set; }
    public Site Site { get; set; }
    public Entity TargetEntity { get; set; }
    public int Ordinal { get; set; }

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

        Name = "The " + Formatting.AddOrdinal(Ordinal) + " Insurrection in " + Site;
        InsurrectionStarted insurrectionStart = Collection.OfType<InsurrectionStarted>().FirstOrDefault();
        if (insurrectionStart != null)
        {
            insurrectionStart.ActualStart = true;
        }
        TargetEntity.AddEventCollection(this);
        Site.AddEventCollection(this);
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        return Name;
    }

    public override string ToString()
    {
        return Name;
    }
}
