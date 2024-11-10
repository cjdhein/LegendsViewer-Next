namespace LegendsViewer.Backend.Contracts;

public class DirectedChordDataDto
{
    public required string Source { get; set; }
    public required string Target { get; set; }
    public int Value { get; set; }
    public required string SourceColor { get; set; }
    public required string TargetColor { get; set; }
    public required string Tooltip { get; set; }
    public required string Href { get; set; }
}
