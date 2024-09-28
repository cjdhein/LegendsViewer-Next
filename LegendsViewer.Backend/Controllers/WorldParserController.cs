using LegendsViewer.Backend.Legends;
using LegendsViewer.Backend.Legends.Bookmarks;
using LegendsViewer.Backend.Legends.Maps;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorldParserController(IWorld worldDataService, IWorldMapImageGenerator worldMapImageGenerator, IBookmarkService bookmarkService) : ControllerBase
{
    private const string FileIdentifierLegendsXml = "-legends.xml";
    private const string FileIdentifierWorldHistoryTxt = "-world_history.txt";
    private const string FileIdentifierWorldMapBmp = "-world_map.bmp";
    private const string FileIdentifierWorldSitesAndPops = "-world_sites_and_pops.txt";
    private const string FileIdentifierLegendsPlusXml = "-legends_plus.xml";

    private readonly IWorld _worldDataService = worldDataService;
    private readonly IWorldMapImageGenerator _worldMapImageGenerator = worldMapImageGenerator;
    private readonly IBookmarkService _bookmarkService = bookmarkService;

    [HttpGet("bookmarks")]
    [ProducesResponseType<List<Bookmark>>( StatusCodes.Status200OK)]
    public ActionResult<List<Bookmark>> GetBookmarks()
    {
        var bookmarks = _bookmarkService.GetAllBookmarks();
        return Ok(bookmarks);
    }

    [HttpPost("parse")]
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
        string regionId = string.Empty;
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
            // Start parsing the XML asynchronously
            await _worldDataService.ParseAsync(xmlFileName, xmlPlusFileName, historyFileName, sitesAndPopsFileName, mapFileName);

            var bookmark = AddBookmark(filePath);

            return Ok(bookmark);
        }
        catch (Exception ex)
        {
            // Handle errors (e.g., file not found, XML parsing errors, etc.)
            return StatusCode(500, $"Error parsing the XML file: {ex.Message}");
        }
    }

    private Bookmark AddBookmark(string filePath)
    {
        var imageData = _worldMapImageGenerator.GenerateMapByteArray(WorldMapImageGenerator.DefaultTileSizeMin);
        var bookmark = new Bookmark
        {
            FilePath = filePath,
            WorldName = _worldDataService.Name,
            WorldAlternativeName = _worldDataService.AlternativeName,
            WorldWidth = _worldDataService.Width,
            WorldHeight = _worldDataService.Height,
            WorldMapImage = imageData,
            State = BookmarkState.Loaded
        };

        _bookmarkService.AddBookmark(bookmark);
        return bookmark;
    }
}
