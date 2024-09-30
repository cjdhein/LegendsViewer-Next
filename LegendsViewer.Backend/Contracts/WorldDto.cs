using LegendsViewer.Backend.Legends;

namespace LegendsViewer.Backend.Contracts;

public class WorldDto(IWorld worldDataService)
{
    public string Name { get; set; } = worldDataService.Name;
    public string AlternativeName { get; set; } = worldDataService.AlternativeName;
    public int Width { get; set; } = worldDataService.Width;
    public int Height { get; set; } = worldDataService.Height;

    public List<SiteMarkerDto> SiteMarkers { get; set; } = worldDataService.Sites.ConvertAll(s => new SiteMarkerDto(s));
}
