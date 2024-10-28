using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.Various;

public class HfPosition(Entity? entity, int? startYear, int? endYear, int positionId, string title)
{
    public Entity? Entity { get; set; } = entity;

    public int PositionId { get; set; } = positionId;
    public string Title { get; set; } = Formatting.InitCaps(title);
    public int? StartYear { get; set; } = startYear;
    public int? EndYear { get; set; } = endYear;

    public string PrintTitle(bool link = true, HistoricalFigure? historicalFigure = null)
    {
        if (Entity != null)
        {
            EntityPosition? position = Entity?.EntityPositions.Find(pos => string.Equals(pos.Name, Title, StringComparison.OrdinalIgnoreCase));
            if (position != null)
            {
                string positionName = (EndYear != null || historicalFigure?.DeathYear > -1 ? "Former " : "");
                positionName += historicalFigure != null ? position.GetTitleByCaste(historicalFigure.Caste) : Title;
                if (Entity != null)
                {
                    return $"The {positionName} of {(link ? Entity.ToLink() : Entity.Name)}";
                }
                return $"{positionName}";
            }
        }
        return $"{Title}";
    }

    public string PrintReign(HistoricalFigure? historicalFigure)
    {
        string startString = StartYear?.ToString() ?? "a time before time";
        string endString = EndYear?.ToString() ?? "the current day";
        if (EndYear == null && historicalFigure?.DeathYear > -1)
        {
            endString = historicalFigure.DeathYear.ToString();
        }
        return $"From {startString} to {endString}";
    }
}
