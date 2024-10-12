using LegendsViewer.Backend.Extensions;
using LegendsViewer.Backend.Legends.Maps;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Contracts;

public class MainCivilizationDto : WorldObjectDto
{
    public string Link { get; set; }
    public byte[]? Thumbnail { get; set; }
    public int SitesCount { get; set; }
    public int CurrentSitesCount { get; set; }
    public int LostSitesCount { get; set; }
    public string Race { get; set; }
    public int? EntityPopulationCount { get; set; }
    public int? EntityPopulationMemberCount { get; set; }

    public MainCivilizationDto(IWorldMapImageGenerator worldMapImageGenerator, Entity mainCivilization) : base(mainCivilization)
    {
        Link = mainCivilization.ToLink();
        Thumbnail = worldMapImageGenerator.GenerateMapByteArray(WorldMapImageGenerator.DefaultTileSizeMin, null, mainCivilization);
        SitesCount = mainCivilization.Sites.Count;
        CurrentSitesCount = mainCivilization.CurrentSites.Count;
        LostSitesCount = mainCivilization.LostSites.Count;
        Race = HtmlStyleUtil.GetCivIconString(
            mainCivilization.Race.NamePlural,
            mainCivilization.LineColor.ToRgbaString(),
            Formatting.GetReadableForegroundColor(mainCivilization.LineColor));
        EntityPopulationCount = mainCivilization.EntityPopulation?.Count;
        EntityPopulationMemberCount = mainCivilization.EntityPopulation?.Members?.Count;
    }
}
