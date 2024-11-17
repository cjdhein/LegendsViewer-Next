namespace LegendsViewer.Backend.Legends.Cytoscape;

public class CytoscapeNodeData
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Tooltip { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string? Parent { get; set; }
    public string ForegroundColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
}
