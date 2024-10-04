using LegendsViewer.Backend.Legends;

namespace LegendsViewer.Backend.Contracts;

public class WorldDto(IWorld worldDataService)
{
    public string Name { get; set; } = worldDataService.Name;
    public string AlternativeName { get; set; } = worldDataService.AlternativeName;
    public int Width { get; set; } = worldDataService.Width;
    public int Height { get; set; } = worldDataService.Height;
    public int CurrentYear { get; set; } = worldDataService.CurrentYear;
    public int CurrentMonth { get; set; } = worldDataService.CurrentMonth;
    public int CurrentDay { get; set; } = worldDataService.CurrentDay;

    public List<SiteMarkerDto> SiteMarkers { get; set; } = worldDataService.Sites.ConvertAll(s => new SiteMarkerDto(s));
}
