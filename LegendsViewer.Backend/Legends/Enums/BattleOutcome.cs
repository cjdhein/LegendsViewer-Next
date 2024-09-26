using System.ComponentModel;

namespace LegendsViewer.Backend.Legends.Enums;

public enum BattleOutcome
{
    Unknown,
    [Description("Attacker Won")]
    AttackerWon,
    [Description("Defender Won")]
    DefenderWon
}