using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class Era : WorldObject
{
    public List<War> Wars { get; set; } = [];
    public int StartYear, EndYear;

    public Era(List<Property> properties, World world)
        : base(properties, world)
    {
        Id = world.Eras.Count;

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "start_year": StartYear = Convert.ToInt32(property.Value); break;
                case "name": Name = property.Value; break;
            }
        }
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        return Name != "" ? Name : $"({StartYear} - {EndYear})";
    }

    public override string ToString() { return Name; }
}
