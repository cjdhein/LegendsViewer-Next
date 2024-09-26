
namespace LegendsViewer.Backend.Legends.Bookmarks
{
    public interface IBookmarkService
    {
        void AddBookmark(Bookmark bookmark);
        List<Bookmark> GetAllBookmarks();
        Bookmark? GetBookmark(string filePath);
    }
}