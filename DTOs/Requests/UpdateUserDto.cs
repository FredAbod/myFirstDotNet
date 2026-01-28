namespace test_dotnet.DTOs.Requests;

/// <summary>
/// DTO for updating an existing user
/// </summary>
public record UpdateUserDto(
    string? Name, 
    string? Email
);
