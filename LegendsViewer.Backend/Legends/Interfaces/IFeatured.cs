using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.Interfaces;

internal interface IFeatured
{
    string PrintFeature(bool link = true, DwarfObject pov = null);
}
