using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;

namespace LegendsViewer.Backend.Legends.EventCollections;

public class Duel : EventCollection
{
    public string Ordinal;
    public Location Coordinates;
    public WorldRegion Region;
    public UndergroundRegion UndergroundRegion;
    public Site Site;
    public HistoricalFigure Attacker;
    public HistoricalFigure Defender;
    public Duel(List<Property> properties, World world)
        : base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "ordinal": Ordinal = string.Intern(property.Value); break;
                case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                case "parent_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                case "attacking_hfid": Attacker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                case "defending_hfid": Defender = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
            }
        }
        //foreach (WorldEvent collectionEvent in Collection) this.AddEvent(collectionEvent);
        if (ParentCollection != null && ParentCollection.GetType() == typeof(Battle))
        {
            foreach (HfDied death in Collection.OfType<HfDied>())
            {
                Battle battle = ParentCollection as Battle;
                War parentWar = battle.ParentCollection as War;
                if (battle.NotableAttackers.Contains(death.HistoricalFigure))
                {
                    battle.AttackerDeathCount++;
                    battle.Attackers.Single(squad => squad.Race == death.HistoricalFigure.Race).Deaths++;

                    if (parentWar != null)
                    {
                        parentWar.AttackerDeathCount++;
                    }
                }
                else if (battle.NotableDefenders.Contains(death.HistoricalFigure))
                {
                    battle.DefenderDeathCount++;
                    battle.Defenders.Single(squad => squad.Race == death.HistoricalFigure.Race).Deaths++;
                    if (parentWar != null)
                    {
                        parentWar.DefenderDeathCount++;
                    }
                }

                if (parentWar != null)
                {
                    parentWar.DeathCount++;
                }
            }
        }
        Attacker.AddEventCollection(this);
        Defender.AddEventCollection(this);
        Region.AddEventCollection(this);
        UndergroundRegion.AddEventCollection(this);
        Site.AddEventCollection(this);
    }

    public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
    {
        return "a duel";
    }
}
