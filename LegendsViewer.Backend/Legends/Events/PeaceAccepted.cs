using LegendsViewer.Backend.Legends.Parser;

namespace LegendsViewer.Backend.Legends.Events;

public class PeaceAccepted : PeaceEfforts
{
    public PeaceAccepted(List<Property> properties, World world)
        : base(properties, world)
    {
        Decision = "accepted";
    }
}