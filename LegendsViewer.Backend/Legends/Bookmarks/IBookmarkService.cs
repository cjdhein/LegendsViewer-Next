
namespace LegendsViewer.Backend.Legends.Bookmarks
{
    public interface IBookmarkService
    {
        Bookmark AddBookmark(Bookmark bookmark);
        List<Bookmark> GetAll();
        Bookmark? GetBookmark(string filePath);
    }
}