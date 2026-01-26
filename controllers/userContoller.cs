using Microsoft.AspNetCore.Mvc;
using test_dotnet.Models;
using test_dotnet.Services;

namespace test_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
   {
     private readonly UserService _userService;
     // Depedency Injection (DI) - service is automatically provided
     public UserController(UserService userService)
     {
       _userService = userService;
     }

     //Get api/user
     [HttpGet]
     public async Task<ActionResult<List<User>>> GetAll()
     {
       var users = await _userService.GetAllAsync();
       return Ok(users);
     }

     //Get api/user/{id}
      [HttpGet("{id}")]
      public async Task<ActionResult<User>> GetById(int id)
      {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
          return NotFound();
        }
        return Ok(user);
      }

      //Post api/user
      [HttpPost]
      public async Task<ActionResult<User>> Create([FromBody] CreateUserDto dto)
      {
        var existing = await _userService.GetEmailAsync(dto.Email);
        if (existing != null)
          return BadRequest(new { message = "Email already in use" });

        var user = new User
        {
          Name = dto.Name,
          Email = dto.Email,
          Password = dto.Password
        };

        await _userService.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
      }

      //Put api/user/{id}
      [HttpPut("{id}")]
      public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
      {
        var user = await _userService.GetByIdAsync(int.Parse(id));
        if (user == null)
        {
          return NotFound();
        }

        user.Name = dto.Name ?? user.Name;
        user.Email = dto.Email ?? user.Email;

        await _userService.UpdateAsync(id, user);
        return Ok(user);
      }
   
    //Delete api/user/{id}
      [HttpDelete("{id}")]
      public async Task<IActionResult> Delete(string id)
      {
        var user = await _userService.GetByIdAsync(int.Parse(id));
        if (user == null)
        {
          return NotFound();
        }

        await _userService.DeleteAsync(id);
        return Ok(new { message = "User deleted successfully"  });
      }

   }

 // DTOs (Data Transfer Objects) - like your request body interfaces
public record CreateUserDto(string Name, string Email, string Password);
public record UpdateUserDto(string? Name, string? Email);