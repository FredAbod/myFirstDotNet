namespace test_dotnet.DTOs.Requests;

/// <summary>
/// DTO for creating a new user (registration)
/// </summary>
public record CreateUserDto(
    string Name, 
    string Email, 
    string Password
);
