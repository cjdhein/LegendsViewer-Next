using LegendsViewer.Backend.Legends.Enums;
using SkiaSharp;

namespace LegendsViewer.Backend.Legends.Maps;

public static class RegionTypeColors
{
    // Dictionary to map RegionType to SKColor
    public static readonly Dictionary<RegionType, SKColor> BaseRegionColors = new Dictionary<RegionType, SKColor>
    {
        { RegionType.Default, new SKColor(169, 169, 169, 0) }, // Default - Transparent
        { RegionType.Mountains, new SKColor(139, 137, 137) },  // Mountains - Dark Gray
        { RegionType.Forest, new SKColor(34, 139, 34) },       // Forest - Dark Green
        { RegionType.Desert, new SKColor(255, 223, 186) },     // Desert - Sandy Yellow
        { RegionType.Ocean, new SKColor(0, 105, 148) },        // Ocean - Deep Blue
        { RegionType.Wetland, new SKColor(47, 79, 79) },       // Wetland - Dark Slate Gray
        { RegionType.Tundra, new SKColor(224, 255, 255) },     // Tundra - Light Cyan
        { RegionType.Glacier, new SKColor(240, 248, 255) },    // Glacier - Very Light Blue
        { RegionType.Grassland, new SKColor(124, 252, 0) },    // Grassland - Light Green
        { RegionType.Hills, new SKColor(85, 107, 47) },        // Hills - Olive Green
        { RegionType.Lake, new SKColor(70, 130, 180) },        // Lake - Steel Blue
        { RegionType.Cavern, new SKColor(105, 105, 105) },     // Cavern - Dark Gray (Deep underground)
        { RegionType.Underworld, new SKColor(128, 0, 128) },   // Underworld - Purple (Mysterious/Demonic theme)
        { RegionType.Magma, new SKColor(255, 69, 0) }          // Magma - Fiery Red (Lava-like)
    };
}