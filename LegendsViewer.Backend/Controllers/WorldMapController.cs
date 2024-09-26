using LegendsViewer.Backend.Legends;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Maps;
using LegendsViewer.Backend.Legends.WorldObjects;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorldMapController(IWorld worldDataService, IWorldMapImageGenerator worldMapImageGenerator) : ControllerBase
{
    private readonly IWorld _worldDataService = worldDataService;
    private readonly IWorldMapImageGenerator _worldMapImageGenerator = worldMapImageGenerator;

    [HttpGet("world/{size}")]
    public IActionResult GetWorldMap(MapSize size = MapSize.Default)
    {
        var imageData = _worldMapImageGenerator.GenerateMapByteArray(GetTileSizeByEnum(size));
        if (imageData == null)
        {
            return NotFound();
        }
        return File(imageData, "image/png");
    }

    [HttpGet("underworld/{size}/{depth}")]
    public IActionResult GetUnderworldMap([FromRoute] MapSize size = MapSize.Default, [FromRoute] int? depth = null)
    {
        var imageData = _worldMapImageGenerator.GenerateMapByteArray(GetTileSizeByEnum(size), depth);
        if (imageData == null)
        {
            return NotFound();
        }
        return File(imageData, "image/png");
    }

    [HttpGet("landmass/{id}/{size}")]
    public IActionResult GetLandmassMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetLandmass(id));
    }

    [HttpGet("mountainpeak/{id}/{size}")]
    public IActionResult GetMountainPeakMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetMountainPeak(id));
    }

    [HttpGet("region/{id}/{size}")]
    public IActionResult GetRegionMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetRegion(id));
    }

    [HttpGet("river/{id}/{size}")]
    public IActionResult GetRiverMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetRiver(id));
    }

    [HttpGet("construction/{id}/{size}")]
    public IActionResult GetWorldConstructionMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetWorldConstruction(id));
    }

    [HttpGet("undergroundregion/{id}/{size}")]
    public IActionResult GetUndergroundRegionMap(int id, MapSize size = MapSize.Default)
    {
        UndergroundRegion? undergroundRegion = _worldDataService.GetUndergroundRegion(id);
        return GetWorldObjectMap(GetTileSizeByEnum(size), undergroundRegion, undergroundRegion?.Depth);
    }

    [HttpGet("site/{id}/{size}")]
    public IActionResult GetSiteMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetSite(id));
    }

    [HttpGet("entity/{id}/{size}")]
    public IActionResult GetEntityMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetEntity(id));
    }

    [HttpGet("artifact/{id}/{size}")]
    public IActionResult GetArtifactMap(int id, MapSize size = MapSize.Default)
    {
        return GetWorldObjectMap(GetTileSizeByEnum(size), _worldDataService.GetArtifact(id));
    }

    private IActionResult GetWorldObjectMap(int tileSize, WorldObject? worldObject, int? depth = null)
    {
        if (worldObject is not IHasCoordinates item)
        {
            return NotFound();
        }
        var imageData = _worldMapImageGenerator.GenerateMapByteArray(tileSize, depth, item);
        if (imageData == null)
        {
            return NotFound();
        }
        return File(imageData, "image/png");
    }

    private static int GetTileSizeByEnum(MapSize size)
    {
        return size switch
        {
            MapSize.Small => WorldMapImageGenerator.DefaultTileSizeMin,
            MapSize.Large => WorldMapImageGenerator.DefaultTileSizeMax,
            _ => WorldMapImageGenerator.DefaultTileSizeMid,
        };
    }
}
