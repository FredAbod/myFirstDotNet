using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace test_dotnet.Auth;

/// <summary>
/// Helper class that creates and validates JWT tokens.
/// Think of it like a "wristband printer" at a concert!
/// </summary>
public class JwtHelper
{
    private readonly IConfiguration _config;

    public JwtHelper(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Creates a JWT token for a user (like printing a concert wristband)
    /// </summary>
    public string GenerateToken(string userId, string email)
    {
        // 1. Get the secret key from settings (like the special ink only we have)
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "DefaultSecretKeyThatIsAtLeast32Chars!")
        );

        // 2. Create signing credentials (proves WE made this token)
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Create claims (info stored IN the token)
        // Claims are like stamps on your wristband: "VIP", "Backstage Access", etc.
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),  // WHO is this?
            new Claim(ClaimTypes.Email, email),            // Their email
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique ID for this token
        };

        // 4. Create the token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],     // WHO made this token
            audience: _config["Jwt:Audience"], // WHO is this token for
            claims: claims,                     // Info inside
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_config["Jwt:ExpireMinutes"] ?? "60")  // When does it expire?
            ),
            signingCredentials: credentials    // Proof it's real
        );

        // 5. Convert to string and return
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
