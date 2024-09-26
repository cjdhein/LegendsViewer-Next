using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Parser;

namespace LegendsViewer.Backend.Legends.Events;

public class DanceFormCreated : FormCreatedEvent
{
    public DanceFormCreated(List<Property> properties, World world) : base(properties, world)
    {
        FormType = FormType.Dance;
    }
}