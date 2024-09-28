
namespace LegendsViewer.Backend.Legends.Bookmarks
{
    public interface IBookmarkService
    {
        void AddBookmark(Bookmark bookmark);
        List<Bookmark> GetAll();
        Bookmark? GetBookmark(string filePath);
    }
}