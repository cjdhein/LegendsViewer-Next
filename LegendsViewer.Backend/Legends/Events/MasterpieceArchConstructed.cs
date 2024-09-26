using LegendsViewer.Backend.Legends.Parser;

namespace LegendsViewer.Backend.Legends.Events;

public class MasterpieceArchConstructed : MasterpieceArch
{
    public MasterpieceArchConstructed(List<Property> properties, World world)
        : base(properties, world)
    {
        Process = "constructed";
    }
}