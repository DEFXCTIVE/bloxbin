
namespace BloxBin.Controllers;

using BloxBin.Models;
using Microsoft.AspNetCore.Mvc;
using BloxBin.Data;

[ApiController]

[Route("[controller]")]
public class BinController : ControllerBase
{
    private readonly BloxBinContext _context;
    public BinController(BloxBinContext context)
    {
        _context = context;
    }
   
    [HttpPost]
    public IActionResult CreateBin()
    {
        // Implementation for creating a bin
        return Ok();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateBin(Guid id)
    {
        // Implementation for updating a bin by ID
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBin(Guid id)
    {
        // Implementation for deleting a bin by ID
        return Ok();
    }

    [HttpPost("{id}/request-key")]
    public IActionResult RequestViewKey(Guid id)
    {
        // Implementation for requesting a view key for a bin by ID
        return Ok();
    }

    [HttpGet("{id}")]
    public ActionResult<Bin> GetBin(Guid id, [FromQuery] string key)
    {
        // Implementation for retrieving a bin by ID with a view key
        return Ok();
    }

   
}