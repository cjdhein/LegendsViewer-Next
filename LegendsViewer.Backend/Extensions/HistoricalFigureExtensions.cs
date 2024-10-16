using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.FamilyTree;
using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Utilities;
using System.Net;

namespace LegendsViewer.Backend.Extensions;

public static class HistoricalFigureExtensions
{
    public static FamilyTreeData CreateFamilyTreeElements(this HistoricalFigure historicalFigure)
    {
        List<FamilyTreeNodeElement> nodes = [CreateFamilyTreeNodeData(historicalFigure, historicalFigure)];
        List<FamilyTreeEdgeElement> edges = [];
        int mothertreesize = 0;
        int fathertreesize = 0;
        GetFamilyDataParents(historicalFigure, historicalFigure, nodes, edges, ref mothertreesize, ref fathertreesize);
        GetFamilyDataChildren(historicalFigure, historicalFigure, nodes, edges);
        var familyTreeData = new FamilyTreeData();
        familyTreeData.Nodes.AddRange(nodes);
        familyTreeData.Edges.AddRange(edges);
        return familyTreeData;
    }

    private static void GetFamilyDataChildren(HistoricalFigure original, HistoricalFigure current, List<FamilyTreeNodeElement> nodes, List<FamilyTreeEdgeElement> edges)
    {
        foreach (HistoricalFigure? child in current.RelatedHistoricalFigures.Where(rel => rel.Type == HistoricalFigureLinkType.Child).Select(rel => rel.HistoricalFigure))
        {
            if (child == null)
            {
                continue;
            }

            FamilyTreeNodeElement node = CreateFamilyTreeNodeData(original, child);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
            FamilyTreeEdgeElement edge = new(new FamilyTreeEdgeData
            {
                Source = current.Id.ToString(),
                Target = child.Id.ToString(),
            });
            if (!edges.Contains(edge))
            {
                edges.Add(edge);
            }
        }
    }

    private static void GetFamilyDataParents(HistoricalFigure original, HistoricalFigure current, List<FamilyTreeNodeElement> nodes, List<FamilyTreeEdgeElement> edges, ref int mothertreesize, ref int fathertreesize)
    {
        foreach (HistoricalFigure? mother in current.RelatedHistoricalFigures.Where(rel => rel.Type == HistoricalFigureLinkType.Mother).Select(rel => rel.HistoricalFigure))
        {
            if (mother == null)
            {
                continue;
            }
            mothertreesize++;
            FamilyTreeNodeElement node = CreateFamilyTreeNodeData(original, mother);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
            FamilyTreeEdgeElement edge = new(new FamilyTreeEdgeData
            {
                Source = mother.Id.ToString(),
                Target = current.Id.ToString(),
            });
            if (!edges.Contains(edge))
            {
                edges.Add(edge);
            }
            if (mothertreesize < 3)
            {
                GetFamilyDataParents(original, mother, nodes, edges, ref mothertreesize, ref fathertreesize);
            }
            mothertreesize--;
        }
        foreach (HistoricalFigure? father in current.RelatedHistoricalFigures.Where(rel => rel.Type == HistoricalFigureLinkType.Father).Select(rel => rel.HistoricalFigure))
        {
            if (father == null)
            {
                continue;
            }
            fathertreesize++;
            FamilyTreeNodeElement node = CreateFamilyTreeNodeData(original, father);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
            FamilyTreeEdgeElement edge = new(new FamilyTreeEdgeData
            {
                Source = father.Id.ToString(),
                Target = current.Id.ToString(),
            });
            if (!edges.Contains(edge))
            {
                edges.Add(edge);
            }
            if (fathertreesize < 3)
            {
                GetFamilyDataParents(original, father, nodes, edges, ref mothertreesize, ref fathertreesize);
            }
            fathertreesize--;
        }
    }

    private static FamilyTreeNodeElement CreateFamilyTreeNodeData(HistoricalFigure original, HistoricalFigure current)
    {
        List<string> classes = original.Equals(current) ? ["current"] : [];

        string title = "";
        if (current.Positions.Count != 0)
        {
            title += original.GetLastNoblePosition();
            title += "\n--------------------\n";
            classes.Add("leader");
        }
        title += current.Race != null && current.Race != original.Race ? current.Race.NameSingular + " " : "";

        string description = "";
        if (current.ActiveInteractions.Any(it => it.Contains("VAMPIRE")))
        {
            description += "Vampire ";
            classes.Add("vampire");
        }
        if (current.ActiveInteractions.Any(it => it.Contains("WEREBEAST")))
        {
            description += "Werebeast ";
            classes.Add("werebeast");
        }
        if (current.ActiveInteractions.Any(it => it.Contains("SECRET") && !it.Contains("ANIMATE") && !it.Contains("UNDEAD_RES")))
        {
            description += "Necromancer ";
            classes.Add("necromancer");
        }
        if (current.Ghost)
        {
            description += "Ghost ";
            classes.Add("ghost");
        }
        if (current.Caste == "Male")
        {
            classes.Add("male");
        }
        else if (current.Caste == "Female")
        {
            classes.Add("female");
        }
        var hfAssignment = current.GetLastAssignmentString();
        var hfHighestSkill = Formatting.InitCaps(current.GetHighestSkillAsString());
        if (!string.IsNullOrWhiteSpace(hfAssignment) &&
            !string.Equals(hfAssignment, "Standard", StringComparison.OrdinalIgnoreCase) &&
            hfAssignment != title &&
            !hfHighestSkill.Contains(hfAssignment))
        {
            description += hfAssignment;
        }
        if (!string.IsNullOrWhiteSpace(hfHighestSkill))
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                description += "\n--------------------\n";
            }
            description += hfHighestSkill;
        }
        if (!string.IsNullOrWhiteSpace(title))
        {
            title += "\n--------------------\n";
        }
        if (!string.IsNullOrWhiteSpace(description))
        {
            description += "\n--------------------\n";
        }
        title += description;
        title += current.Name;
        if (!current.Alive)
        {
            title += "\n\n✝";
            classes.Add("dead");
        }

        FamilyTreeNodeData nodaData = new()
        {
            Id = current.Id.ToString(),
            Label = WebUtility.HtmlEncode(title),
            Href = $"/hf/{current.Id}"
        };
        return new FamilyTreeNodeElement(nodaData)
        {
            Classes = classes
        };
    }
}
