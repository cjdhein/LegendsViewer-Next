using LegendsViewer.Backend.Legends.EventCollections;
using LegendsViewer.Backend.Legends.Events;

namespace LegendsViewer.Backend.Extensions;

public static class EventCollectionExtensions
{
    public static void EnrichWithOccasionEventData(this EventCollection eventCollection)
    {
        var occasionEvent = eventCollection.Events.OfType<OccasionEvent>().FirstOrDefault();
        if (occasionEvent != null)
        {
            if (eventCollection.Site == null)
            {
                eventCollection.Site = occasionEvent.Site;
            }
            if (eventCollection.Region == null)
            {
                eventCollection.Region = occasionEvent.Region;
            }
            if (eventCollection.UndergroundRegion == null)
            {
                eventCollection.UndergroundRegion = occasionEvent.UndergroundRegion;
            }
            if (occasionEvent.Civ != null)
            {
                eventCollection.Subtype = occasionEvent.Civ.ToLink(true, eventCollection);
            }
            if (occasionEvent.EntityOccasion != null && !string.IsNullOrEmpty(occasionEvent.EntityOccasion.Name))
            {
                eventCollection.Name += $" of {occasionEvent.EntityOccasion.Name}";
            }
        }
    }
}
