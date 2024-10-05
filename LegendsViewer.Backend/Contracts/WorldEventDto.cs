using LegendsViewer.Backend.Legends;
using LegendsViewer.Backend.Legends.Events;

namespace LegendsViewer.Backend.Contracts;

public class WorldEventDto(WorldEvent worldEvent, DwarfObject? pointOfView = null)
{
    public int Id { get; set; } = worldEvent.Id;
    public int Year { get; set; } = worldEvent.Year;
    public int Month { get; set; } = worldEvent.Month;
    public int Day { get; set; } = worldEvent.Day;
    public string MonthName { get; set; } = worldEvent.MonthName;
    public string Date { get; set; } = worldEvent.Date;
    public string Type { get; set; } = worldEvent.Type;
    public string Html { get; set; } = worldEvent.Print(true, pointOfView);
}
