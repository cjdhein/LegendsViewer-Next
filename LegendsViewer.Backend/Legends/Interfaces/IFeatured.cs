namespace LegendsViewer.Backend.Legends.Interfaces;

internal interface IFeatured
{
    string PrintFeature(bool link = true, DwarfObject? pov = null);
}
