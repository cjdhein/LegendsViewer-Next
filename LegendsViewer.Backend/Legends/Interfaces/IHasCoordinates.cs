using LegendsViewer.Backend.Legends.Various;

namespace LegendsViewer.Backend.Legends.Interfaces;

public interface IHasCoordinates
{
    List<Location> Coordinates { get; } // legends_plus.xml
}
