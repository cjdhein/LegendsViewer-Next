namespace LegendsViewer.Backend.Legends.FamilyTree;

public class FamilyTreeNodeElement
{
    public FamilyTreeNodeData Data { get; set; }
    public List<string> Classes { get; set; } = [];

    public FamilyTreeNodeElement(FamilyTreeNodeData data)
    {
        Data = data;
    }
}
