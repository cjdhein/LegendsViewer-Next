using LegendsViewer.Backend.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

[ApiController]
[Route("api/version")]
public class VersionController(IWebHostEnvironment env) : ControllerBase
{
    private readonly IWebHostEnvironment _env = env;

    [HttpGet]
    [ProducesResponseType<VersionDto>(StatusCodes.Status200OK)]
    public ActionResult<VersionDto> GetVersion()
    {
        var version = System.Reflection.Assembly.GetExecutingAssembly()
            .GetName().Version;
        var versionString = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.0.0";

        // Append "-dev" if running in the Development environment
        if (_env.IsDevelopment())
        {
            versionString += "-dev";
        }
        return Ok(new VersionDto { Version = versionString });
    }
}
