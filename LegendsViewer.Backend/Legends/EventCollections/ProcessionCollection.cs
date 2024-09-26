using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class ProcessionCollection : EventCollection
{
    public string Ordinal;

    public ProcessionCollection(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = string.Intern(property.Value); break;
            }
        }
    }
    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        return "a procession";
    }
}
