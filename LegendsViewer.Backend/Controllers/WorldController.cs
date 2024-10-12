using LegendsViewer.Backend.Contracts;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.Maps;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorldController(IWorld worldDataService, IWorldMapImageGenerator worldMapImageGenerator) : ControllerBase
{
    private readonly IWorld _worldDataService = worldDataService;
    private readonly IWorldMapImageGenerator _worldMapImageGenerator = worldMapImageGenerator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<WorldDto> Get()
    {
        return Ok(new WorldDto(_worldDataService, _worldMapImageGenerator));
    }
}
