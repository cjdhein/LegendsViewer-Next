using System.ComponentModel;

namespace LegendsViewer.Backend.Legends.Enums;

public enum StructureType
{
    Unknown,
    [Description("Mead Hall")]
    MeadHall,
    Market,
    Keep,
    Temple,
    Dungeon,
    [Description("Tavern & Inn")]
    InnTavern,
    Tomb,
    [Description("Underworld Spire")]
    UnderworldSpire,
    Library,
    Tower,
    [Description("Counting House")]
    CountingHouse,
    Guildhall
}
