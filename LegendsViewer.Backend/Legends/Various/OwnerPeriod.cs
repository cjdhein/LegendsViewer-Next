using LegendsViewer.Backend.Legends.WorldObjects;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.Various;

public class OwnerPeriod
{
    [JsonIgnore]
    public Site Site { get; set; }
    public string? SiteToLink => Site?.ToLink(true);

    [JsonIgnore]
    public HistoricalFigure? Founder { get; set; }
    public string? FounderToLink => Founder?.ToLink(true);

    [JsonIgnore]
    public Entity? Owner { get; set; }
    public string? OwnerToLink => Owner?.ToLink(true);

    [JsonIgnore]
    public Entity? Ender { get; set; }
    public string? EnderToLink => Ender?.ToLink(true);

    [JsonIgnore]
    public HistoricalFigure? Destroyer { get; set; }
    public string? DestroyerToLink => Destroyer?.ToLink(true);

    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string? StartCause { get; set; }
    public string? EndCause { get; set; }

    public OwnerPeriod(Site site, Entity? owner, int startYear, string startCause, HistoricalFigure? founder = null)
    {
        Site = site;
        Owner = owner;
        StartYear = startYear;
        StartCause = startCause;
        EndYear = -1;

        Founder = founder;

        Owner?.AddOwnedSite(this);
    }
}
