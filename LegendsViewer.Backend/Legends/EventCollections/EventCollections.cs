using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Legends.EventCollections;

public abstract class EventCollection : WorldObject
{
    public int StartYear { get; set; } = -1;
    public int StartSeconds72 { get; set; } = -1;
    public int EndYear { get; set; } = -1;
    public int EndSeconds72 { get; set; } = -1;

    public WorldRegion? Region { get; set; }
    public UndergroundRegion? UndergroundRegion { get; set; }
    public Site? Site { get; set; }

    public EventCollection? ParentCollection { get; set; }
    public List<int> CollectionIDs { get; set; } = [];
    public bool Notable { get; set; } = true;
    public List<WorldEvent> AllEvents => GetSubEvents();

    protected EventCollection(List<Property> properties, World world) :base(properties, world)
    {
        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "start_year": StartYear = Convert.ToInt32(property.Value); property.Known = true; break;
                case "start_seconds72": StartSeconds72 = Convert.ToInt32(property.Value); property.Known = true; break;
                case "end_year": EndYear = Convert.ToInt32(property.Value); property.Known = true; break;
                case "end_seconds72": EndSeconds72 = Convert.ToInt32(property.Value); property.Known = true; break;
                case "type": Type = Formatting.InitCaps(property.Value); property.Known = true; break;
                case "event":
                    WorldEvent? collectionEvent = world.GetEvent(Convert.ToInt32(property.Value));
                    //Some Events don't exist in the XML now with 34.01? 
                    ///TODO: Investigate EventCollection Events that don't exist in the XML, check if they exist in game or if this is just errors.
                    if (collectionEvent != null)
                    {
                        collectionEvent.ParentCollection = this;
                        Events.Add(collectionEvent); property.Known = true;
                    }
                    break;
                case "eventcol": CollectionIDs.Add(Convert.ToInt32(property.Value)); property.Known = true; break;
                case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
            }
        }

        if (Site?.Region != null && Region == null)
        {
            Region = Site.Region;
        }

        Region?.AddEventCollection(this);
        UndergroundRegion?.AddEventCollection(this);
        Site?.AddEventCollection(this);
    }

    public string GetYearTime(bool start = true)
    {
        int year;
        int seconds72;
        if (start)
        {
            year = StartYear;
            seconds72 = StartSeconds72;
        }
        else
        {
            year = EndYear;
            seconds72 = EndSeconds72;
        }
        if (year == -1)
        {
            return "In a time before time, ";
        }

        string yearTime = $"In {year}, ";
        if (seconds72 == -1)
        {
            return yearTime;
        }

        int partOfMonth = seconds72 % 100800;
        string partOfMonthString = "";
        if (partOfMonth <= 33600)
        {
            partOfMonthString = "early ";
        }
        else if (partOfMonth <= 67200)
        {
            partOfMonthString = "mid";
        }
        else if (partOfMonth <= 100800)
        {
            partOfMonthString = "late ";
        }

        int season = seconds72 % 403200;
        string seasonString = "";
        if (season < 100800)
        {
            seasonString = "spring";
        }
        else if (season < 201600)
        {
            seasonString = "summer";
        }
        else if (season < 302400)
        {
            seasonString = "autumn";
        }
        else if (season < 403200)
        {
            seasonString = "winter";
        }

        return $"{yearTime}{partOfMonthString}{seasonString}";
    }

    public List<WorldEvent> GetSubEvents()
    {
        List<WorldEvent> events = [];
        foreach (EventCollection subCollection in EventCollections)
        {
            events.AddRange(subCollection.GetSubEvents());
        }

        events.AddRange(Events);
        return events.OrderBy(collectionEvent => collectionEvent.Id).ToList();
    }
}
