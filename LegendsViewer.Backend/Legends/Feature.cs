using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends;

public class Feature : WorldObject
{
    public string Type { get; set; } // legends_plus.xml
    public int Reference { get; set; } // legends_plus.xml

    public Feature(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "type": Type = string.Intern(property.Value); break;
                case "reference": Reference = Convert.ToInt32(property.Value); break;
            }
        }
    }

    public override string ToString() { return Type; }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        return Type;
    }
}
