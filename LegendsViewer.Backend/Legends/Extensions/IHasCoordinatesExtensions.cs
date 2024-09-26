using LegendsViewer.Backend.Legends.Interfaces;

namespace LegendsViewer.Backend.Legends.Extensions;

public static class IHasCoordinatesExtensions
{
    public static int MinX(this IHasCoordinates obj)
    {
        return obj.Coordinates.Min(c => c.X);
    }

    public static int MaxX(this IHasCoordinates obj)
    {
        return obj.Coordinates.Max(c => c.X);
    }

    public static int MinY(this IHasCoordinates obj)
    {
        return obj.Coordinates.Min(c => c.Y);
    }

    public static int MaxY(this IHasCoordinates obj)
    {
        return obj.Coordinates.Max(c => c.Y);
    }

    public static int Width(this IHasCoordinates obj)
    {
        return obj.MaxX() + 1 - obj.MinX();
    }

    public static int Height(this IHasCoordinates obj)
    {
        return obj.MaxY() + 1 - obj.MinY();
    }

    public static double CenterX(this IHasCoordinates obj)
    {
        return obj.Coordinates.Average(c => c.X);
    }

    public static double CenterY(this IHasCoordinates obj)
    {
        return obj.Coordinates.Average(c => c.Y);
    }
}
