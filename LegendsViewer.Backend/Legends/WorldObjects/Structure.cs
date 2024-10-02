using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class Structure : WorldObject
{
    public string Name { get; set; } = "Structure";
    public string AltName { get; set; } = "";
    public StructureType Type { get; set; } // legends_plus.xml

    [JsonIgnore]
    public List<int> InhabitantIDs { get; set; } = [];

    [JsonIgnore]
    public List<HistoricalFigure> Inhabitants { get; set; } = [];
    public List<string> InhabitantLinks => Inhabitants.ConvertAll(x => x.ToLink(true, this));

    [JsonIgnore]
    public int DeityId { get; set; } // legends_plus.xml

    [JsonIgnore]
    public HistoricalFigure? Deity { get; set; } // legends_plus.xml
    public string? DeityToLink => Deity?.ToLink(true);

    [JsonIgnore]
    public int ReligionId { get; set; } // legends_plus.xml

    [JsonIgnore]
    public Entity? Religion { get; set; } // legends_plus.xml
    public string? ReligionToLink => Religion?.ToLink(true);

    public StructureSubType StructureSubType { get; set; } // legends_plus.xml

    [JsonIgnore]
    public int DeityType { get; set; } // TODO legends_plus.xml

    [JsonIgnore]
    public HistoricalFigure? Owner { get; set; } // resolved from site properties
    public string? OwnerToLink => Owner?.ToLink(true);

    [JsonIgnore]
    public string TypeAsString
    {
        get
        {
            return StructureSubType != StructureSubType.Unknown ? StructureSubType.GetDescription() : Type.GetDescription();
        }
    }

    public string Icon { get; set; }

    [JsonIgnore]
    public int EntityId { get; set; }

    [JsonIgnore]
    public Entity? Entity { get; set; }
    public string? EntityToLink => Entity?.ToLink(true);

    [JsonIgnore]
    public Site? Site { get; set; }
    public string? SiteToLink => Site?.ToLink(true);

    public int GlobalId { get; set; }

    [JsonIgnore]
    public List<int> CopiedArtifactIds { get; set; } = [];

    [JsonIgnore]
    public List<Artifact> CopiedArtifacts { get; set; } = [];
    public List<string> CopiedArtifactLinks => CopiedArtifacts.ConvertAll(x => x.ToLink(true, this));

    public Structure(List<Property> properties, World world, Site site)
        : base(properties, world)
    {
        Name = "UNKNOWN STRUCTURE";
        AltName = "";
        DeityId = -1;
        ReligionId = -1;
        EntityId = -1;
        DeityType = -1;

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "local_id": Id = Convert.ToInt32(property.Value); break;
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "name2": AltName = Formatting.InitCaps(property.Value); break;
                case "inhabitant":
                    InhabitantIDs.Add(Convert.ToInt32(property.Value));
                    break;
                case "deity":
                case "worship_hfid":
                    DeityId = Convert.ToInt32(property.Value);
                    break;
                case "deity_type":
                    DeityType = Convert.ToInt32(property.Value);
                    break;
                case "entity_id": EntityId = Convert.ToInt32(property.Value); break;
                case "religion": ReligionId = Convert.ToInt32(property.Value); break;
                case "copied_artifact_id": CopiedArtifactIds.Add(Convert.ToInt32(property.Value)); break;
                case "dungeon_type":
                    switch (property.Value)
                    {
                        case "0": StructureSubType = StructureSubType.Dungeon; break;
                        case "1": StructureSubType = StructureSubType.Sewers; break;
                        case "2": StructureSubType = StructureSubType.Catacombs; break;
                        default:
                            property.Known = false;
                            break;
                    }
                    break;
                case "subtype":
                    switch (property.Value)
                    {
                        case "standard": break;
                        case "catacombs": StructureSubType = StructureSubType.Catacombs; break;
                        case "sewers": StructureSubType = StructureSubType.Sewers; break;
                        default:
                            property.Known = false;
                            break;
                    }
                    break;
                case "type":
                    switch (property.Value)
                    {
                        case "mead_hall":
                        case "mead hall":
                            Type = StructureType.MeadHall; break;
                        case "market": Type = StructureType.Market; break;
                        case "keep": Type = StructureType.Keep; break;
                        case "temple": Type = StructureType.Temple; break;
                        case "dungeon": Type = StructureType.Dungeon; break;
                        case "tomb": Type = StructureType.Tomb; break;
                        case "inn_tavern":
                        case "inn tavern":
                            Type = StructureType.InnTavern; break;
                        case "underworld_spire":
                        case "underworld spire":
                            Type = StructureType.UnderworldSpire; break;
                        case "library": Type = StructureType.Library; break;
                        case "tower": Type = StructureType.Tower; break;
                        case "counting_house":
                        case "counting house":
                            Type = StructureType.CountingHouse; break;
                        case "guildhall": Type = StructureType.Guildhall; break;
                        default:
                            property.Known = false;
                            break;
                    }
                    break;
            }
        }
        string icon = "";
        switch (Type)
        {
            case StructureType.MeadHall:
                icon = HtmlStyleUtil.GetIconString("glass-mug-variant");
                break;
            case StructureType.Market:
                icon = HtmlStyleUtil.GetIconString("scale-balance");
                break;
            case StructureType.Keep:
                icon = HtmlStyleUtil.GetIconString("castle");
                break;
            case StructureType.Temple:
                icon = HtmlStyleUtil.GetIconString("temple-buddhist");
                break;
            case StructureType.Dungeon:
                icon = HtmlStyleUtil.GetIconString("tunnel");
                break;
            case StructureType.InnTavern:
                icon = HtmlStyleUtil.GetIconString("food-turkey");
                break;
            case StructureType.Tomb:
                icon = HtmlStyleUtil.GetIconString("grave-stone");
                break;
            case StructureType.UnderworldSpire:
                icon = HtmlStyleUtil.GetIconString("star-three-points");
                break;
            case StructureType.Library:
                icon = HtmlStyleUtil.GetIconString("library");
                break;
            case StructureType.Tower:
                icon = HtmlStyleUtil.GetIconString("chess-rook");
                break;
            case StructureType.CountingHouse:
                icon = HtmlStyleUtil.GetIconString("bank-outline");
                break;
            case StructureType.Guildhall:
                icon = HtmlStyleUtil.GetIconString("town-hall");
                break;
            default:
                icon = "";
                break;
        }
        Icon = icon;
        Site = site;

        GlobalId = world.Structures.Count;
        world.Structures.Add(this);
    }

    public void Resolve(World world)
    {
        if (InhabitantIDs.Count > 0)
        {
            foreach (int inhabitantId in InhabitantIDs)
            {
                if (world.GetHistoricalFigure(inhabitantId) is HistoricalFigure inhabitant)
                {
                    Inhabitants.Add(inhabitant);
                }
            }
        }
        if (DeityId != -1)
        {
            Deity = world.GetHistoricalFigure(DeityId);
        }
        if (ReligionId != -1)
        {
            Religion = world.GetEntity(ReligionId);
        }
        if (EntityId != -1)
        {
            Entity = world.GetEntity(EntityId);
        }
        if (Deity != null && Religion != null)
        {
            if (!Religion.Worshipped.Contains(Deity))
            {
                Religion.Worshipped.Add(Deity);
                Deity.DedicatedStructures.Add(this);
            }
        }
        if (CopiedArtifactIds.Count > 0)
        {
            foreach (int copiedArtifactId in CopiedArtifactIds)
            {
                if(world.GetArtifact(copiedArtifactId) is Artifact artifact)
                {
                    CopiedArtifacts.Add(artifact);
                }
            }
        }
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string title = "";
            if (StructureSubType != StructureSubType.Unknown)
            {
                title += StructureSubType.GetDescription();
            }
            else
            {
                title += Type.GetDescription();
            }
            title += "&#13";
            title += "Events: " + Events.Count;

            string linkedString = pov != this
                ? $"{HtmlStyleUtil.GetAnchorString(Icon, "structure", GlobalId, title, Name)}"
                : $"{HtmlStyleUtil.GetAnchorString(Icon, "structure", GlobalId, title, HtmlStyleUtil.CurrentDwarfObject(Name))}";
            return linkedString;
        }
        return Icon + Name;
    }

    public override string ToString() { return Name; }

    public override string GetIcon()
    {
        return Icon;
    }
}
