using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class PerformanceCollection : EventCollection
{
    public int Ordinal;
    public PerformanceCollection(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = Convert.ToInt32(property.Value); break;
            }
        }
    }
    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        return "a performance";
    }
}
