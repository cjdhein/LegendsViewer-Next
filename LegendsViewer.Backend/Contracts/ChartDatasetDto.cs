namespace LegendsViewer.Backend.Contracts;

public class ChartDatasetDto
{
    public string Label { get; set; } = default!;
    public List<int> Data { get; set; } = [];
    public List<string> BorderColor { get; set; } = [];
    public List<string> BackgroundColor { get; set; } = [];
}
