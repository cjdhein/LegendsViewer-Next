namespace LegendsViewer.Backend.Legends.FamilyTree;

public class FamilyTreeEdgeElement
{
    public FamilyTreeEdgeData Data { get; set; }
    public List<string> Classes { get; set; } = [];

    public FamilyTreeEdgeElement(FamilyTreeEdgeData data)
    {
        Data = data;
    }
}
