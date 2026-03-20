
namespace BloxBin.Controllers;

using Microsoft.AspNetCore.Mvc;
using BloxBin.Models;
using BloxBin.Data;
using BloxBin.DTOs;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

[ApiController]

[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly BloxBinContext _context;
    private readonly IConfiguration _configuration;

    public UserController(BloxBinContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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
    public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        // Implementation for updating a user by ID
        User? user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        if (updateUserDto.Name != null)
        {
            user.Name = updateUserDto.Name;
        }

        if (updateUserDto.Password != null)
        {
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
        }

        _context.SaveChanges();

        return NoContent();
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
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // Implementation for user login
        User? user = _context.Users.FirstOrDefault(u => u.Name == loginDto.Name);
        if (user == null)
        {
            return Unauthorized();
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
        {
            return Unauthorized();
        }

        // Generate JWT token (not implemented here)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}