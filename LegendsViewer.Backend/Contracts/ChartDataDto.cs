namespace LegendsViewer.Backend.Contracts;

public class ChartDataDto
{
    public List<string> Labels { get; set; } = [];
    public List<ChartDatasetDto> Datasets { get; set; } = [];
}
