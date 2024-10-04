using LegendsViewer.Backend.Legends;
using LegendsViewer.Backend.Legends.Bookmarks;
using LegendsViewer.Backend.Legends.Maps;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookmarkController(IWorld worldDataService, IWorldMapImageGenerator worldMapImageGenerator, IBookmarkService bookmarkService) : ControllerBase
{
    public const string FileIdentifierLegendsXml = "-legends.xml";

    private const string FileIdentifierWorldHistoryTxt = "-world_history.txt";
    private const string FileIdentifierWorldMapBmp = "-world_map.bmp";
    private const string FileIdentifierWorldSitesAndPops = "-world_sites_and_pops.txt";
    private const string FileIdentifierLegendsPlusXml = "-legends_plus.xml";
    private readonly IWorld _worldDataService = worldDataService;
    private readonly IWorldMapImageGenerator _worldMapImageGenerator = worldMapImageGenerator;
    private readonly IBookmarkService _bookmarkService = bookmarkService;

    [HttpGet]
    [ProducesResponseType<List<Bookmark>>( StatusCodes.Status200OK)]
    public ActionResult<List<Bookmark>> Get()
    {
        var bookmarks = _bookmarkService.GetAll();
        return Ok(bookmarks);
    }

    [HttpGet("{filePath}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Bookmark> Get([FromRoute] string filePath)
    {
        var item = _bookmarkService.GetBookmark(filePath);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost("loadByFullPath")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Bookmark>> ParseWorldXml([FromBody] string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
        {
            return BadRequest("Invalid file path.");
        }
        FileInfo fileInfo = new(filePath);
        if (string.IsNullOrWhiteSpace(fileInfo.DirectoryName))
        {
            return BadRequest("Invalid directory.");
        }
        string directoryName = fileInfo.DirectoryName;
        string regionName = string.Empty;
        string timestamp = string.Empty;
        string regionId;
        if (fileInfo.Name.Contains(FileIdentifierLegendsXml))
        {
            regionId = fileInfo.Name.Replace(FileIdentifierLegendsXml, "");
        }
        else if (fileInfo.Name.Contains(FileIdentifierLegendsPlusXml))
        {
            regionId = fileInfo.Name.Replace(FileIdentifierLegendsPlusXml, "");
        }
        else
        {
            return BadRequest("Invalid file name.");
        }
        int firstHyphenIndex = regionId.IndexOf('-');
        if (firstHyphenIndex != -1)
        {
            regionName = regionId[..firstHyphenIndex]; // Extract the region name
            timestamp = regionId[(firstHyphenIndex + 1)..]; // Extract the timestamp part

            // Extract year, month, and day as integers
            _worldDataService.CurrentYear = int.Parse(timestamp.Substring(0, 5));   // First 5 characters represent the year
            _worldDataService.CurrentMonth = int.Parse(timestamp.Substring(6, 2));  // Characters 6 and 7 represent the month
            _worldDataService.CurrentDay = int.Parse(timestamp.Substring(9, 2));    // Characters 9 and 10 represent the day
        }

        var xmlFileName = Directory.EnumerateFiles(directoryName, regionId + FileIdentifierLegendsXml).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(xmlFileName))
        {
            return BadRequest("Invalid XML file");
        }
        var xmlPlusFileName = Directory.EnumerateFiles(directoryName, regionId + FileIdentifierLegendsPlusXml).FirstOrDefault();
        var historyFileName = Directory.EnumerateFiles(directoryName, regionId + FileIdentifierWorldHistoryTxt).FirstOrDefault();
        var sitesAndPopsFileName = Directory.EnumerateFiles(directoryName, regionId + FileIdentifierWorldSitesAndPops).FirstOrDefault();
        var mapFileName = Directory.EnumerateFiles(directoryName, regionId + FileIdentifierWorldMapBmp).FirstOrDefault();

        try
        {
            _worldDataService.Clear();
            _worldMapImageGenerator.Clear();

            // Start parsing the XML asynchronously
            await _worldDataService.ParseAsync(xmlFileName, xmlPlusFileName, historyFileName, sitesAndPopsFileName, mapFileName);

            var bookmark = AddBookmark(filePath, regionName, timestamp);

            return Ok(bookmark);
        }
        catch (Exception ex)
        {
            // Handle errors (e.g., file not found, XML parsing errors, etc.)
            return StatusCode(500, $"Error parsing the XML file: {ex.Message}");
        }
    }

    [HttpPost("loadByFolderAndFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Bookmark>> ParseWorldXml([FromBody] string folderPath, string fileName)
    {
        var fullPath = Path.Combine(folderPath, fileName);
        if (!Path.Exists(fullPath))
        {
            return BadRequest("File does not exist!");
        }
        return await ParseWorldXml(fullPath);
    }

    private Bookmark AddBookmark(string filePath, string regionName, string timestamp)
    {
        var imageData = _worldMapImageGenerator.GenerateMapByteArray(WorldMapImageGenerator.DefaultTileSizeMin);
        var bookmark = new Bookmark
        {
            FilePath = filePath.Replace(timestamp, BookmarkService.TimestampPlaceholder),
            WorldName = _worldDataService.Name,
            WorldAlternativeName = _worldDataService.AlternativeName,
            WorldRegionName = regionName,
            WorldTimestamps = [timestamp],
            WorldWidth = _worldDataService.Width,
            WorldHeight = _worldDataService.Height,
            WorldMapImage = imageData,
            State = BookmarkState.Loaded,
            LoadedTimestamp = timestamp,
            LatestTimestamp = timestamp
        };

        return _bookmarkService.AddBookmark(bookmark);
    }
}
