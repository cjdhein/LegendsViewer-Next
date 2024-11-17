namespace LegendsViewer.Backend.Legends.Cytoscape;

public class CytoscapeNodeElement
{
    public CytoscapeNodeData Data { get; set; }
    public List<string> Classes { get; set; } = [];

    public CytoscapeNodeElement(CytoscapeNodeData data)
    {
        Data = data;
    }
}
