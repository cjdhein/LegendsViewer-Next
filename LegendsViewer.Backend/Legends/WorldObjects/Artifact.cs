using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Legends.Various;
using LegendsViewer.Backend.Utilities;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class Artifact : WorldObject, IHasCoordinates
{
    public string Icon { get; set; } = HtmlStyleUtil.GetIconString("diamond-stone");

    public string Item { get; set; }
    [JsonIgnore]
    public HistoricalFigure? Creator { get; set; }
    public string? CreatorLink => Creator?.ToLink(true, this);

    [JsonIgnore]
    public HistoricalFigure? Holder { get; set; }
    public string? HolderLink => Holder?.ToLink(true, this);

    [JsonIgnore]
    public Structure? Structure { get; set; }
    public string? StructureLink => Structure?.ToLink(true, this);

    public int HolderId { get; set; }
    public string? Description { get; set; } // legends_plus.xml
    public string? Material { get; set; } // legends_plus.xml
    public int PageCount { get; set; } // legends_plus.xml

    [JsonIgnore]
    public List<int> WrittenContentIds { get; set; } = [];

    [JsonIgnore]
    public List<WrittenContent> WrittenContents { get; set; } = [];
    public List<string> WrittenContentLinks => WrittenContents.ConvertAll(x => x.ToLink(true, this));

    public int AbsTileX { get; set; }
    public int AbsTileY { get; set; }
    public int AbsTileZ { get; set; }
    public List<Location> Coordinates { get; set; } = [];

    [JsonIgnore]
    public Site? Site { get; set; }
    public string? SiteLink => Site?.ToLink(true, this);

    [JsonIgnore]
    public WorldRegion? Region { get; set; }
    public string? RegionLink => Region?.ToLink(true, this);

    public Artifact(List<Property> properties, World world)
        : base(properties, world)
    {
        Name = "Untitled";
        Type = "Unknown";
        Subtype = "";

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "name": Name = Formatting.InitCaps(property.Value); break;
                case "item":
                    if (property.SubProperties != null)
                    {
                        property.Known = true;
                        foreach (Property subProperty in property.SubProperties)
                        {
                            switch (subProperty.Name)
                            {
                                case "name_string":
                                    Item = Formatting.InitCaps(subProperty.Value);
                                    break;
                                case "page_number":
                                    PageCount = Convert.ToInt32(subProperty.Value);
                                    break;
                                case "page_written_content_id":
                                case "writing_written_content_id":
                                    if (!WrittenContentIds.Contains(Convert.ToInt32(subProperty.Value)))
                                    {
                                        WrittenContentIds.Add(Convert.ToInt32(subProperty.Value));
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Item = Formatting.InitCaps(property.Value);
                    }
                    break;
                case "item_type": Type = Formatting.InitCaps(property.Value); break;
                case "item_subtype": Subtype = string.Intern(property.Value); break;
                case "item_description": Description = Formatting.InitCaps(property.Value); break;
                case "mat": Material = string.Intern(property.Value); break;
                case "page_count": PageCount = Convert.ToInt32(property.Value); break;
                case "abs_tile_x": AbsTileX = Convert.ToInt32(property.Value); break;
                case "abs_tile_y": AbsTileY = Convert.ToInt32(property.Value); break;
                case "abs_tile_z": AbsTileZ = Convert.ToInt32(property.Value); break;
                case "writing": if (!WrittenContentIds.Contains(Convert.ToInt32(property.Value))) { WrittenContentIds.Add(Convert.ToInt32(property.Value)); } break;
                case "site_id":
                    Site = world.GetSite(Convert.ToInt32(property.Value));
                    break;
                case "subregion_id":
                    Region = world.GetRegion(Convert.ToInt32(property.Value));
                    break;
                case "holder_hfid":
                    HolderId = Convert.ToInt32(property.Value);
                    break;
                case "structure_local_id":
                    Structure = Site?.Structures.Find(structure => structure.Id == Convert.ToInt32(property.Value));
                    break;
            }
        }
        if (AbsTileX > 0 && AbsTileY > 0)
        {
            Coordinates.Add(new Location(AbsTileX / 816, AbsTileY / 816));
        }
        else if (Site != null)
        {
            Coordinates.AddRange(Site.Coordinates);
        }
    }

    public void Resolve(World world)
    {
        if (HolderId > 0)
        {
            Holder = world.GetHistoricalFigure(HolderId);
            if (Holder?.HoldingArtifacts.Contains(this) == false)
            {
                Holder.HoldingArtifacts.Add(this);
            }
        }
        if (WrittenContentIds.Count > 0)
        {
            WrittenContents = [];
            foreach (var writtenContentId in WrittenContentIds)
            {
                WrittenContent? writtenContent = world.GetWrittenContent(writtenContentId);
                if (writtenContent != null)
                {
                    WrittenContents.Add(writtenContent);
                }
            }
        }
    }

    public override string ToString() { return Name; }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string title = "Artifact" + (!string.IsNullOrEmpty(Type) ? ", " + Type : "");
            title += "&#13";
            title += "Events: " + Events.Count;
            return pov != this
                ? HtmlStyleUtil.GetAnchorString(Icon, "artifact", Id, title, Name)
                : HtmlStyleUtil.GetAnchorCurrentString(Icon, title, HtmlStyleUtil.CurrentDwarfObject(Name));
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
