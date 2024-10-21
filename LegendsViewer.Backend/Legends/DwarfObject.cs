using LegendsViewer.Backend.Legends.Events;

namespace LegendsViewer.Backend.Legends;

public abstract class DwarfObject
{
    public int Id { get; set; }
    public virtual string GetIcon()
    {
        return "";
    }

    public virtual string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        return "";
    }
}
