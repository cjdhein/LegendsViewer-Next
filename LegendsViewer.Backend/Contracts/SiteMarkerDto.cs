using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Contracts;

public class SiteMarkerDto(Site site)
{
    public int Id { get; set; } = site.Id;
    public string Name { get; set; } = site.Name;
    public SiteType Type { get; set; } = site.SiteType;
    public List<Location> Coordinates { get; set; } = site.Coordinates;
}
