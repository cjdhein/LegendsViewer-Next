using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Parser;

namespace LegendsViewer.Backend.Legends.Events;

public class Performance : OccasionEvent
{
    public Performance(List<Property> properties, World world) : base(properties, world)
    {
        OccasionType = OccasionType.Performance;
    }
}