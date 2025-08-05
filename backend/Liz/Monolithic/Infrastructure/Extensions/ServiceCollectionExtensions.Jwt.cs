using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = GetStrictJwtKeyOrThrow(configuration);
        var jwtSettings = configuration.GetSection("Jwt");
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
    }

    private static string GetStrictJwtKeyOrThrow(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"] ?? throw new ArgumentException("JWT Key is not configured in app settings.json");
        if (Encoding.UTF8.GetByteCount(key) < 32)
        {
            throw new ArgumentException("JWT Key must be at least 32 bytes long");
        }
        return key;
    }
}
