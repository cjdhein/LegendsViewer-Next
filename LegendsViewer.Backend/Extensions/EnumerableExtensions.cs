using System.Reflection;

namespace LegendsViewer.Backend.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> SortByProperty<T>(this IEnumerable<T> source, string? sortKey, string? sortOrder)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (sortKey == null || sortOrder == null)
        {
            return source;
        }

        // Get the property by name
        PropertyInfo? propertyInfo = typeof(T).GetProperty(sortKey, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{sortKey}' not found on type '{typeof(T).Name}'");
        }

        // Sort in ascending or descending order based on sortOrder
        if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            return source.OrderByDescending(x => propertyInfo.GetValue(x, null));
        }
        else
        {
            return source.OrderBy(x => propertyInfo.GetValue(x, null));
        }
    }
}
