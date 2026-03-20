
namespace BloxBin.Controllers;

using BloxBin.Models;
using Microsoft.AspNetCore.Mvc;
using BloxBin.Data;
using Microsoft.AspNetCore.Authorization;
using BloxBin.DTOs;
using System.Security.Claims;

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
    [Authorize]
    public IActionResult CreateBin([FromBody] CreateBinDto createBinDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        User? user = _context.Users.Find(userId);
        if (user == null)
        {
            return Unauthorized();
        }
        Bin newBin = new Bin
        {
            Name = createBinDto.Name,
            Content = createBinDto.Content,
            ExpiresAt = createBinDto.ExpiresAt,
            IsPrivate = createBinDto.IsPrivate,
            CreatedAt = DateTime.UtcNow,
            OwnerId = userId,
        };
        _context.Bins.Add(newBin);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetBin), new { id = newBin.Id }, new BinResponseDto
        {
            Id = newBin.Id,
            Name = newBin.Name,
            Content = newBin.Content,
            CreatedAt = newBin.CreatedAt,
            ExpiresAt = newBin.ExpiresAt,
            IsPrivate = newBin.IsPrivate,
            OwnerId = newBin.OwnerId
        });
    }

    [HttpPatch("{id}")]
    [Authorize]

    public IActionResult UpdateBin(Guid id, [FromBody] UpdateBinDto updateBinDto)
    {
        // Implementation for updating a bin by ID
        User? user = _context.Users.Find(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
        if (user == null)
        {
            return Unauthorized();
        }
        Bin? bin = _context.Bins.Find(id);
        if (bin == null)
        {
            return NotFound();
        }
        if (bin.OwnerId != user.Id)
        {
            return Forbid();
        }

        // Update the bin properties with the values from the DTO
        if (updateBinDto.Name != null)
        {
            bin.Name = updateBinDto.Name;
        }
        if (updateBinDto.Content != null)
        {
            bin.Content = updateBinDto.Content;
        }
        if (updateBinDto.ExpiresAt != null)
        {
            bin.ExpiresAt = updateBinDto.ExpiresAt;
        }
        if (updateBinDto.IsPrivate != null)
        {
            bin.IsPrivate = (bool)updateBinDto.IsPrivate;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]

    public IActionResult DeleteBin(Guid id)
    {
        User? user = _context.Users.Find(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
        if (user == null)
        {
            return Unauthorized();
        }
        Bin? bin = _context.Bins.Find(id);
        if (bin == null)
        {
            return NotFound();
        }
        if (bin.OwnerId != user.Id)
        {
            return Forbid();
        }
        _context.Bins.Remove(bin);
        _context.SaveChanges();
        // Implementation for deleting a bin by ID
        return NoContent();
    }

    [HttpPost("{id}/request-key")]
    [Authorize]

    public IActionResult RequestViewKey(Guid id)
    {
        // Implementation for requesting a view key for a bin by ID
        Bin? bin = _context.Bins.Find(id);
        if (bin == null)
        {
            return NotFound();
        }
        if (bin.IsPrivate)
        {
            User? user = _context.Users.Find(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null)
            {
                return Unauthorized();
            }
            if (bin.OwnerId != user.Id)
            {
                return Forbid();
            }
            string rawKey = Guid.NewGuid().ToString();
            string keyHash = BCrypt.Net.BCrypt.HashPassword(rawKey);
            ViewKey viewKey = new ViewKey
            {
                KeyHash = keyHash,
                BinId = bin.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1) // Example expiration time
            };
            _context.ViewKeys.Add(viewKey);
            _context.SaveChanges();
            return Ok(new { ViewKey = rawKey });

        }
        return BadRequest("Bin is not private, no key required.");
    }

    [HttpGet("{id}")]
    public ActionResult<BinResponseDto> GetBin(Guid id, [FromQuery] string? key)
    {

        Bin? bin = _context.Bins.Find(id);
        if (bin == null)
        {
            return NotFound();
        }
        if (bin.ExpiresAt.HasValue && bin.ExpiresAt < DateTime.UtcNow)
        {
            return NotFound();
        }

        if (bin.IsPrivate)
        {
            // Check if the provided key is valid (not implemented here)
            ViewKey? viewKey = _context.ViewKeys.FirstOrDefault(vk => vk.BinId == bin.Id && vk.ExpiresAt > DateTime.UtcNow);
            if (viewKey == null)
            {
                return Unauthorized();
            }
            bool isKeyValid = BCrypt.Net.BCrypt.Verify(key, viewKey.KeyHash);
            if (!isKeyValid)
            {
                return Unauthorized();
            }
            _context.ViewKeys.Remove(viewKey);
            _context.SaveChanges();
        }

        return Ok(new BinResponseDto
        {
            Id = bin.Id,
            Name = bin.Name,
            Content = bin.Content,
            CreatedAt = bin.CreatedAt,
            ExpiresAt = bin.ExpiresAt,
            IsPrivate = bin.IsPrivate,
            OwnerId = bin.OwnerId
        });
    }


}