using test_dotnet.Models;

namespace test_dotnet.DTOs.Responses;

/// <summary>
/// Safe user data to return in responses (NO PASSWORD!)
/// This is what the API sends back - never the raw User entity
/// </summary>
public class UserResponseDto
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Converts a User entity to a safe response DTO
    /// </summary>
    public static UserResponseDto FromUser(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
