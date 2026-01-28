using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace test_dotnet.Auth;

/// <summary>
/// Extension methods for configuring JWT authentication
/// Keeps all JWT-related code in one place!
/// </summary>
public static class JwtConfiguration
{
    /// <summary>
    /// Adds JWT authentication to the application
    /// Call this in Program.cs: builder.Services.AddJwtAuthentication(builder.Configuration)
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            // Set JWT as the default authentication scheme
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Configure how tokens are validated
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // ✅ Check who made the token (must match Jwt:Issuer)
                ValidateIssuer = true,
                
                // ✅ Check who the token is for (must match Jwt:Audience)
                ValidateAudience = true,
                
                // ✅ Check if token is expired
                ValidateLifetime = true,
                
                // ✅ Check the signature is real (uses Jwt:Key)
                ValidateIssuerSigningKey = true,

                // The expected values from appsettings.json
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        configuration["Jwt:Key"] ?? "DefaultSecretKey32CharactersLong!"
                    )
                )
            };
        });

        return services;
    }
}
