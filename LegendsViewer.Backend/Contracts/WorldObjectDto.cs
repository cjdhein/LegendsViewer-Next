using LegendsViewer.Backend.Legends;

namespace LegendsViewer.Backend.Contracts;

public class WorldObjectDto(WorldObject worldObject, DwarfObject? pointOfView = null)
{
    public int Id { get; set; } = worldObject.Id;
    public string Name { get; set; } = worldObject.Name;
    public string? Type { get; set; } = worldObject.Type;
    public string? Subtype { get; set; } = worldObject.Subtype;
    public string Html { get; set; } = worldObject.ToLink(true, pointOfView);
    public int EventCount { get; set; } = worldObject.EventCount;
}
