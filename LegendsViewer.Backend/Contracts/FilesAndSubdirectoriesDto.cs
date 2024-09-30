namespace LegendsViewer.Backend.Contracts;

public class FilesAndSubdirectoriesDto
{
    public string CurrentDirectory { get; set; } = string.Empty;
    public string? ParentDirectory { get; set; }
    public string[] Subdirectories { get; set; } = [];
    public string[] Files { get; set; } = [];
}
