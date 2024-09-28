namespace LegendsViewer.Backend.Legends.Bookmarks;

using System.Text.Json;

public class BookmarkService : IBookmarkService
{
    private const string BookmarkFileName = "bookmarks.json";

    private readonly Dictionary<string, Bookmark> _bookmarks;
    private readonly string _bookmarkFilePath;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public BookmarkService()
    {
        // Determine the platform-independent file path
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataPath, "LegendsViewer"); // Adjust app folder name
        Directory.CreateDirectory(appFolder); // Ensure directory exists

        _bookmarkFilePath = Path.Combine(appFolder, BookmarkFileName);

        // Load bookmarks from disk if the file exists
        if (File.Exists(_bookmarkFilePath))
        {
            string json = File.ReadAllText(_bookmarkFilePath);
            _bookmarks = JsonSerializer.Deserialize<Dictionary<string, Bookmark>>(json) ?? [];
            foreach (var bookmark in _bookmarks.Values)
            {
                bookmark.State = BookmarkState.Default;   
            }
        }
        else
        {
            _bookmarks = [];
        }
    }

    public void AddBookmark(Bookmark bookmark)
    {
        if (!_bookmarks.ContainsKey(bookmark.FilePath))
        {
            _bookmarks[bookmark.FilePath] = bookmark;
            SaveBookmarksToFile();
        }
    }

    public List<Bookmark> GetAll()
    {
        return [.. _bookmarks.Values];
    }

    public Bookmark? GetBookmark(string filePath)
    {
        return _bookmarks.TryGetValue(filePath, out var bookmark) ? bookmark : null;
    }

    private void SaveBookmarksToFile()
    {
        string json = JsonSerializer.Serialize(_bookmarks, _jsonSerializerOptions);
        File.WriteAllText(_bookmarkFilePath, json);
    }
}
