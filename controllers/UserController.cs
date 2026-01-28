using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using test_dotnet.Models;
using test_dotnet.Services;
using test_dotnet.Auth;
using test_dotnet.DTOs.Requests;
using test_dotnet.DTOs.Responses;

namespace test_dotnet.Controllers;

/// <summary>
/// User API Controller - All responses use unified ApiResponse format
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtHelper _jwtHelper;

    public UserController(UserService userService, JwtHelper jwtHelper)
    {
        _userService = userService;
        _jwtHelper = jwtHelper;
    }

    // ========================================
    // GET api/user - Get all users
    // ========================================
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserResponseDto>>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        var response = users.Select(u => UserResponseDto.FromUser(u)).ToList();
        
        return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(
            response, 
            $"Retrieved {response.Count} users"
        ));
    }

    // ========================================
    // GET api/user/{id} - Get user by ID
    // ========================================
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<UserResponseDto>.ErrorResponse("User not found"));
        }
        
        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(
            UserResponseDto.FromUser(user)
        ));
    }

    // ========================================
    // POST api/user - Create new user (Register)
    // ========================================
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Create([FromBody] CreateUserDto dto)
    {
        var existing = await _userService.GetEmailAsync(dto.Email);
        if (existing != null)
        {
            return BadRequest(ApiResponse<UserResponseDto>.ErrorResponse(
                "Email already in use",
                new List<string> { "Please use a different email address" }
            ));
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password
        };

        await _userService.CreateAsync(user);

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            ApiResponse<UserResponseDto>.SuccessResponse(
                UserResponseDto.FromUser(user),
                "User created successfully"
            )
        );
    }

    // ========================================
    // PUT api/user/{id} - Update user
    // ========================================
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Update(string id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userService.GetByIdAsync(int.Parse(id));
        if (user == null)
        {
            return NotFound(ApiResponse<UserResponseDto>.ErrorResponse("User not found"));
        }

        user.Name = dto.Name ?? user.Name;
        user.Email = dto.Email ?? user.Email;

        await _userService.UpdateAsync(id, user);

        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(
            UserResponseDto.FromUser(user),
            "User updated successfully"
        ));
    }

    // ========================================
    // DELETE api/user/{id} - Delete user
    // ========================================
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id)
    {
        var user = await _userService.GetByIdAsync(int.Parse(id));
        if (user == null)
        {
            return NotFound(ApiResponse.ErrorResponse("User not found"));
        }

        await _userService.DeleteAsync(id);
        return Ok(ApiResponse.SuccessResponse("User deleted successfully"));
    }

    // ========================================
    // POST api/user/login - Login and get JWT
    // ========================================
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto dto)
    {
        var user = await _userService.GetEmailAsync(dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse(
                "Invalid credentials",
                new List<string> { "Email or password is incorrect" }
            ));
        }

        var token = _jwtHelper.GenerateToken(user.Id ?? "", user.Email);

        var loginResponse = new LoginResponseDto
        {
            Token = token,
            User = UserResponseDto.FromUser(user)
        };

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
            loginResponse,
            "Login successful"
        ));
    }
}
