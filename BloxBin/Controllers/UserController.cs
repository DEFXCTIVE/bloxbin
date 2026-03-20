
namespace BloxBin.Controllers;
using Microsoft.AspNetCore.Mvc;
using BloxBin.Models;
using BloxBin.Data;
using BloxBin.DTOs;
[ApiController]

[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly BloxBinContext _context;

    public UserController(BloxBinContext context)
    {
        _context = context;
    }
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
    {
        // Implementation for creating a user
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        User newUser = new User
        {
            Name = createUserDto.Name,
            HashedPassword = hashedPassword
        };
        try
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            // Log the exception (not implemented here)
            return StatusCode(500, "An error occurred while creating the user.");
        }
        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, new UserResponseDto { Id = newUser.Id, Name = newUser.Name });
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(Guid id)
    {
        // Implementation for retrieving a user by ID
        return _context.Users.Find(id) is User user ? Ok(new UserResponseDto { Id = user.Id, Name = user.Name }) : NotFound();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateUser(Guid id)
    {
        // Implementation for updating a user by ID
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(Guid id)
    {
        // Implementation for deleting a user by ID
        User? user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPost("Login")]
    public IActionResult Login()
    {
        // Implementation for user login
        return Ok();
    }
}