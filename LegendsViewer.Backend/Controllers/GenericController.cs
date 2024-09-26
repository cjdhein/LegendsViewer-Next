using LegendsViewer.Backend.Contracts;
using LegendsViewer.Backend.Legends;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class GenericController<T>(List<T> allElements, Func<int, T?> getById) : ControllerBase where T : WorldObject
{
    private const int DefaultPageSize = 10;
    protected readonly List<T> AllElements = allElements;
    protected readonly Func<int, T?> GetById = getById;

    // Default values for pageNumber and pageSize
    [HttpGet]
    public IActionResult Get(int pageNumber = 1, int pageSize = DefaultPageSize)
    {
        // Validate pagination parameters
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size must be greater than zero.");
        }

        // Get total number of elements
        int totalElements = AllElements.Count;

        // Calculate how many elements to skip based on the page number and size
        var paginatedElements = AllElements
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Create a response object to include pagination metadata
        var response = new PaginatedResponse<T>
        {
            Items = paginatedElements,
            TotalCount = totalElements,
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalPages = (int)Math.Ceiling(totalElements / (double)pageSize)
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var item = GetById(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpGet("count")]
    public IActionResult GetCount()
    {
        return Ok(AllElements.Count);
    }

    [HttpGet("{id}/events")]
    public IActionResult GetEvents(int id, int pageNumber = 1, int pageSize = DefaultPageSize)
    {
        WorldObject? item = GetById(id);
        if (item == null)
        {
            return NotFound();
        }

        // Validate pagination parameters
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size must be greater than zero.");
        }

        // Get total number of elements
        int totalElements = item.Events.Count;

        // Calculate how many elements to skip based on the page number and size
        var paginatedElements = item.Events
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new WorldEventDto(e, item))
            .ToList();

        // Create a response object to include pagination metadata
        var response = new PaginatedResponse<WorldEventDto>
        {
            Items = paginatedElements,
            TotalCount = totalElements,
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalPages = (int)Math.Ceiling(totalElements / (double)pageSize)
        };

        return Ok(response);
    }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
}