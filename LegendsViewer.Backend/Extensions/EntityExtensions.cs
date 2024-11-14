using LegendsViewer.Backend.Legends.Cytoscape;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;

namespace LegendsViewer.Backend.Extensions;

public static class EntityExtensions
{
    public static CytoscapeNodeElement GetCytoscapeNode(this Entity entity, Entity? current = null)
    {
        string label = $"{entity.Race.NamePlural}";
        label += entity.IsCiv ? "\n──────── ✶ ────────" : "";
        label += $"\n{entity}\n{entity.EntityType.GetDescription()}";
        var node = new CytoscapeNodeElement(new CytoscapeNodeData
        {
            Id = $"node-{entity.Id}",
            Label = label,
            Href = $"/entity/{entity.Id}",
            BackgroundColor = entity.LineColor.ToRgbaString(0.6f),
            ForegroundColor = Formatting.GetReadableForegroundColor(entity.LineColor),
            Parent = entity.CurrentCiv != null ? $"node-{entity.CurrentCiv.Id}" : null
        });
        if (current != null && current == entity)
        {
            node.Classes.Add("current");
        }
        if (entity.IsCiv)
        {
            node.Classes.Add("is-civilization");
        }
        return node;
    }
}
