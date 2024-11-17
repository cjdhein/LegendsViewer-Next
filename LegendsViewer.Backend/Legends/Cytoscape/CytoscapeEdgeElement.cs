namespace LegendsViewer.Backend.Legends.Cytoscape;

public class CytoscapeEdgeElement
{
    public CytoscapeEdgeData Data { get; set; }
    public List<string> Classes { get; set; } = [];

    public CytoscapeEdgeElement(CytoscapeEdgeData data)
    {
        Data = data;
    }
}
