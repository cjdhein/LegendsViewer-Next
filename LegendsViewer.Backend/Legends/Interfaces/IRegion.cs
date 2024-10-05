using LegendsViewer.Backend.Legends.Enums;

namespace LegendsViewer.Backend.Legends.Interfaces;

internal interface IRegion : IHasCoordinates
{
    int Id { get; }
    int? Depth { get; set; }
    RegionType RegionType { get; set; }
}