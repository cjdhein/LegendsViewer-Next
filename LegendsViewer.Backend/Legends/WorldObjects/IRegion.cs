using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Interfaces;

namespace LegendsViewer.Backend.Legends.WorldObjects;

internal interface IRegion : IHasCoordinates
{
    int Id { get; }
    int? Depth { get; set; }
    RegionType Type { get; set; }
}