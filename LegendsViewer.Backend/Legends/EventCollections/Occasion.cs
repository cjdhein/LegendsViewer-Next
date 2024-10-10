using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Occasion : EventCollection
{
    public int Ordinal { get; set; } = -1;
    public Entity? Civ { get; set; }
    public int OccasionId { get; set; }
    public EntityOccasion? EntityOccasion { get; set; }

    public Occasion(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                case "ordinal": Ordinal = Convert.ToInt32(property.Value); break;
                case "occasion_id": OccasionId = Convert.ToInt32(property.Value); break;
            }
        }
        if (Civ?.Occassions.Count > 0 == true)
        {
            EntityOccasion = Civ.Occassions.ElementAt(OccasionId);
        }
        Civ?.AddEventCollection(this);
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        return "an occasion";
    }
}
