using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TaskSync.Application.Interfaces;
using TaskSync.Application.Interfaces.Security;
using TaskSync.Infrastructure.Persistence;
using TaskSync.Infrastructure.Security;

namespace TaskSync.Infrastructure;

public static class AddInfrastructureService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(config["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey: Missing in configuration file"))),

                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer: Missing in configuration file"),

                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience: Missing in configuration file"),

                    ValidateLifetime = true,
                };
            });

        services.AddAuthorization();

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IPasswordHasher, Argon2idPasswordHasher>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
}
