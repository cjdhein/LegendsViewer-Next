namespace LegendsViewer.Backend.Legends.Cytoscape;

public class CytoscapeEdgeData
{
    public string Source { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string ForegroundColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Tooltip { get; set; } = string.Empty;
    public int Width { get; set; } = 1;
}
