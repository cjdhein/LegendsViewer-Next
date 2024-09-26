using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends;

public class EntityPopulation : WorldObject
{
    public CreatureInfo Race { get; set; } // legends_plus.xml
    public int Count { get; set; } // legends_plus.xml
    public int EntityId { get; set; } // legends_plus.xml

    [JsonIgnore]
    public Entity? Entity { get; set; } // legends_plus.xml
    public string? EntityLink => Entity?.ToLink(true, this);

    [JsonIgnore]
    public List<HistoricalFigure> Members { get; set; }
    public List<string> MemberLinks => Members.ConvertAll(x => x.ToLink(true, this));

    public EntityPopulation(List<Property> properties, World world)
        : base(properties, world)
    {
        EntityId = -1;
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "race":
                    var raceCount = property.Value.Split(':');
                    Race = world.GetCreatureInfo(raceCount[0]);
                    Count = Convert.ToInt32(raceCount[1]);
                    break;
                case "civ_id":
                    EntityId = property.ValueAsInt();
                    break;
            }
        }
    }
}
