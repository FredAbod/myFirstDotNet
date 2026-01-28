namespace test_dotnet.DTOs.Responses;

/// <summary>
/// Login response - contains token and user data
/// Note: No "Message" field - that's in ApiResponse wrapper
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserResponseDto? User { get; set; }
}
                