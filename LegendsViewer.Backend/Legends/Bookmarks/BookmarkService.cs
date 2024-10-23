namespace LegendsViewer.Backend.Legends.Bookmarks;

using LegendsViewer.Backend.Controllers;
using System.IO;
using System.Text.Json;

public class BookmarkService : IBookmarkService
{
    public const string TimestampPlaceholder = "{TIMESTAMP}";
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
                ResetBookmark(bookmark);
            }
        }
        else
        {
            _bookmarks = [];
        }
    }

    private static void ResetBookmark(Bookmark bookmark)
    {
        bookmark.State = BookmarkState.Default;
        bookmark.LoadedTimestamp = null;
        bookmark.LatestTimestamp = bookmark.WorldTimestamps.Order().LastOrDefault();
    }

    public Bookmark AddBookmark(Bookmark bookmark)
    {
        ResetAllBookmarks();
        if (_bookmarks.TryGetValue(bookmark.FilePath, out var existingBookmark))
        {
            foreach (var timestamp in bookmark.WorldTimestamps)
            {
                if (!existingBookmark.WorldTimestamps.Contains(timestamp))
                {
                    existingBookmark.WorldTimestamps.Add(timestamp);
                }
            }
            existingBookmark.WorldName = bookmark.WorldName;
            existingBookmark.WorldAlternativeName = bookmark.WorldAlternativeName;
            existingBookmark.WorldMapImage = bookmark.WorldMapImage;
            existingBookmark.State = BookmarkState.Loaded;
            existingBookmark.LoadedTimestamp = bookmark.LoadedTimestamp;
            existingBookmark.LatestTimestamp = bookmark.LatestTimestamp;
            SaveBookmarksToFile();
            return existingBookmark;
        }
        else
        {
            _bookmarks[bookmark.FilePath] = bookmark;
            SaveBookmarksToFile();
            return bookmark;
        }
    }

    private void ResetAllBookmarks()
    {
        foreach (var bookmark in _bookmarks.Values) { ResetBookmark(bookmark); }
    }

    public List<Bookmark> GetAll()
    {
        return [.. _bookmarks.Values];
    }

    public Bookmark? GetBookmark(string filePath)
    {
        string timestamp = string.Empty;
        if (filePath.Contains(BookmarkController.FileIdentifierLegendsXml))
        {
            string regionId = filePath.Replace(BookmarkController.FileIdentifierLegendsXml, "");
            int firstHyphenIndex = regionId.IndexOf('-');
            if (firstHyphenIndex != -1)
            {
                timestamp = regionId[(firstHyphenIndex + 1)..]; // Extract the timestamp part
            }
        }
        if (string.IsNullOrWhiteSpace(timestamp))
        {
            return null;
        }
        return _bookmarks.TryGetValue(filePath.Replace(timestamp, TimestampPlaceholder), out var bookmark) ? bookmark : null;
    }

    private void SaveBookmarksToFile()
    {
        string json = JsonSerializer.Serialize(_bookmarks, _jsonSerializerOptions);
        File.WriteAllText(_bookmarkFilePath, json);
    }
}
