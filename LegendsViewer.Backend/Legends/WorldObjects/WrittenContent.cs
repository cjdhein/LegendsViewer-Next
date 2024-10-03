using LegendsViewer.Backend.Legends.Enums;
using LegendsViewer.Backend.Legends.Events;
using LegendsViewer.Backend.Legends.Extensions;
using LegendsViewer.Backend.Legends.Parser;
using LegendsViewer.Backend.Utilities;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend.Legends.WorldObjects;

public class WrittenContent : WorldObject
{
    public static readonly string Icon = HtmlStyleUtil.GetIconString("bookshelf");

    private string _name = default!;
    public string Name
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_name))
            {
                return _name;
            }

            var type = Type.GetDescription();
            _name = "An untitled " + type.ToLower();
            return _name;
        }
        set => _name = value;
    } // legends_plus.xml

    public int PageStart { get; set; } // legends_plus.xml
    public int PageEnd { get; set; } // legends_plus.xml
    public WrittenContentType Type { get; set; } // legends_plus.xml

    [JsonIgnore]
    public HistoricalFigure? Author { get; set; } // legends_plus.xml
    public int AuthorId { get; set; } // legends_plus.xml
    public List<string> Styles { get; set; } = [];
    public List<Reference> References { get; set; } = [];
    public string TypeAsString => Type.GetDescription();
    public int PageCount => PageEnd - PageStart + 1;
    public int AuthorRoll { get; set; }
    public int FormId { get; set; }

    public WrittenContent(List<Property> properties, World world)
        : base(properties, world)
    {
        FormId = -1;
        AuthorId = -1;

        foreach (Property property in properties)
        {
            switch (property.Name)
            {
                case "title": Name = Formatting.InitCaps(property.Value); break;
                case "page_start": PageStart = Convert.ToInt32(property.Value); break;
                case "page_end": PageEnd = Convert.ToInt32(property.Value); break;
                case "reference":
                    property.Known = true;
                    if (property.SubProperties != null)
                    {
                        References.Add(new Reference(property.SubProperties, world));
                    }

                    break;
                case "form":
                case "type":
                    switch (property.Value)
                    {
                        case "Autobiography":
                        case "autobiography":
                            Type = WrittenContentType.Autobiography; break;
                        case "Biography":
                        case "biography":
                            Type = WrittenContentType.Biography; break;
                        case "Chronicle":
                        case "chronicle":
                            Type = WrittenContentType.Chronicle; break;
                        case "Dialog":
                        case "dialog":
                            Type = WrittenContentType.Dialog; break;
                        case "Essay":
                        case "essay":
                            Type = WrittenContentType.Essay; break;
                        case "Guide":
                        case "guide":
                            Type = WrittenContentType.Guide; break;
                        case "Letter":
                        case "letter":
                            Type = WrittenContentType.Letter; break;
                        case "Manual":
                        case "manual":
                            Type = WrittenContentType.Manual; break;
                        case "Novel":
                        case "novel":
                            Type = WrittenContentType.Novel; break;
                        case "Play":
                        case "play":
                            Type = WrittenContentType.Play; break;
                        case "Poem":
                        case "poem":
                            Type = WrittenContentType.Poem; break;
                        case "ShortStory":
                        case "short story":
                            Type = WrittenContentType.ShortStory; break;
                        case "MusicalComposition":
                        case "musical composition":
                            Type = WrittenContentType.MusicalComposition; break;
                        case "Choreography":
                        case "choreography":
                            Type = WrittenContentType.Choreography; break;
                        case "CulturalHistory":
                        case "cultural history":
                            Type = WrittenContentType.CulturalHistory; break;
                        case "StarChart":
                        case "star chart":
                            Type = WrittenContentType.StarChart; break;
                        case "ComparativeBiography":
                        case "comparative biography":
                            Type = WrittenContentType.ComparativeBiography; break;
                        case "CulturalComparison":
                        case "cultural comparison":
                            Type = WrittenContentType.CulturalComparison; break;
                        case "Atlas":
                        case "atlas":
                            Type = WrittenContentType.Atlas; break;
                        case "TreatiseOnTechnologicalEvolution":
                        case "treatise on technological evolution":
                            Type = WrittenContentType.TreatiseOnTechnologicalEvolution; break;
                        case "AlternateHistory":
                        case "alternate history":
                            Type = WrittenContentType.AlternateHistory; break;
                        case "StarCatalogue":
                        case "star catalogue":
                            Type = WrittenContentType.StarCatalogue; break;
                        case "Dictionary":
                        case "dictionary":
                            Type = WrittenContentType.Dictionary; break;
                        case "Genealogy":
                        case "genealogy":
                            Type = WrittenContentType.Genealogy; break;
                        case "Encyclopedia":
                        case "encyclopedia":
                            Type = WrittenContentType.Encyclopedia; break;
                        case "BiographicalDictionary":
                        case "biographical dictionary":
                            Type = WrittenContentType.BiographicalDictionary; break;
                        default:
                            Type = WrittenContentType.Unknown;
                            if (!int.TryParse(property.Value.Replace("unknown ", ""), out _))
                            {
                                property.Known = false;
                            }
                            break;
                    }
                    break;
                case "author":
                case "author_hfid":
                    AuthorId = Convert.ToInt32(property.Value);
                    Author = world.GetHistoricalFigure(AuthorId);
                    break;
                case "style":
                    var style = property.Value.Contains(":")
                        ? string.Intern(property.Value.Substring(0, property.Value.IndexOf(":", StringComparison.Ordinal))).ToLower()
                        : string.Intern(property.Value).ToLower();
                    if (Styles.Contains(style.Replace(" ", "")))
                    {
                        Styles.Remove(style.Replace(" ", ""));
                        Styles.Add(style);
                    }
                    else if (Styles.All(s => s.Replace(" ", "") != style))
                    {
                        Styles.Add(style);
                    }
                    break;
                case "author_roll":
                    AuthorRoll = Convert.ToInt32(property.Value);
                    break;
                case "form_id":
                    FormId = Convert.ToInt32(property.Value);
                    break;
            }
        }
    }

    public override string ToString()
    {
        return Name;
    }

    public override string ToLink(bool link = true, DwarfObject? pov = null, WorldEvent? worldEvent = null)
    {
        if (link)
        {
            string? type = null;
            if (Type != WrittenContentType.Unknown)
            {
                type = Type.GetDescription();
            }
            string title = "Written Content";
            title += string.IsNullOrWhiteSpace(type) ? "" : ", " + type;
            title += "&#13";
            title += "Events: " + Events.Count;

            string linkedString = pov != this
                ? HtmlStyleUtil.GetAnchorString(Icon, "writtencontent", Id, title, Name)
                : HtmlStyleUtil.GetAnchorCurrentString(Icon, title, HtmlStyleUtil.CurrentDwarfObject(Name));
            return linkedString;
        }
        return Name;
    }

    public override string GetIcon()
    {
        return Icon;
    }
}
