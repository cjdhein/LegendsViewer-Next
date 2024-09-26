namespace LegendsViewer.Backend.Legends;

public class Population
{
    public bool IsMainRace => World.MainRaces.ContainsKey(Race);

    public bool IsOutcasts => Race.NamePlural.Contains("Outcasts");

    public bool IsPrisoners => Race.NamePlural.Contains("Prisoners");

    public bool IsSlaves => Race.NamePlural.Contains("Slaves");

    public bool IsVisitors => Race.NamePlural.Contains("Visitors");

    public bool IsAnimalPeople => Race.NamePlural.Contains(" Men") && !IsSlaves && !IsPrisoners && !IsOutcasts && !IsVisitors;

    public CreatureInfo Race { get; set; }
    public int Count { get; set; }

    public Population(CreatureInfo type, int count)
    {
        Race = type;
        Count = count;
    }
}
