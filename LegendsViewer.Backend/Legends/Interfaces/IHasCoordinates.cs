using LegendsViewer.Backend.Legends.Various;

namespace LegendsViewer.Backend.Legends.Interfaces;

public interface IHasCoordinates
{
    List<Location> Coordinates { get; set; } // legends_plus.xml
}
