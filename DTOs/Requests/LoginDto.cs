namespace test_dotnet.DTOs.Requests;

/// <summary>
/// DTO for user login
/// </summary>
public record LoginDto(
    string Email, 
    string Password
);
