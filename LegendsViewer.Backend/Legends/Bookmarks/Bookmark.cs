namespace LegendsViewer.Backend.Legends.Bookmarks;

public class Bookmark
{
    public string FilePath { get; set; } = "";
    public string WorldName { get; set; } = "";
    public string WorldAlternativeName { get; set; } = "";
    public int WorldWidth { get; set; }
    public int WorldHeight { get; set; }
    public byte[]? WorldMapImage { get; set; }
}
