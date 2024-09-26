using System.ComponentModel;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends;

public enum EntityEntityLinkType // legends_plus.xml
{
    [Description("Subgroup")]
    Child,
    [Description("Parentgroup")]
    Parent,
    Religious,
    Unknown
}

public class EntityEntityLink // legends_plus.xml
{
    public EntityEntityLinkType Type { get; set; }
    public Entity? Target { get; set; }
    public int Strength { get; set; }

    public EntityEntityLink(List<Property> properties, World world)
    {
        Type = EntityEntityLinkType.Unknown;
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "type":
                    switch (property.Value)
                    {
                        case "CHILD":
                            Type = EntityEntityLinkType.Child;
                            break;
                        case "PARENT":
                            Type = EntityEntityLinkType.Parent;
                            break;
                        case "RELIGIOUS":
                            Type = EntityEntityLinkType.Religious;
                            break;
                        default:
                            world.ParsingErrors.Report("Unknown Entity Entity Link Type: " + property.Value);
                            break;
                    }
                    break;
                case "target":
                    Target = world.GetEntity(Convert.ToInt32(property.Value));
                    break;
                case "strength":
                    Strength = Convert.ToInt32(property.Value);
                    break;
            }
        }
    }
}