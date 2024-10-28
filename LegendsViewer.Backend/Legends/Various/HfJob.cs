using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.Various;

public class HfJob(Site? site, int? startYear, int? endYear, string title)
{
    public Site? Site { get; set; } = site;

    public string Title { get; set; } = title;
    public int? StartYear { get; set; } = startYear;
    public int? EndYear { get; set; } = endYear;
}
