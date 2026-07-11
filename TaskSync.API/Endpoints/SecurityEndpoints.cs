using MediatR;
using TaskSync.Application.Features.Security.Commands;

namespace TaskSync.API.Endpoints;

public static class SecurityEndpoints
{
    public static void MapSecurityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication Endpoints");

        group.MapPost("/login", async (HttpContext context,
        LoginUserCommand command,
        ISender sender,
        CancellationToken cancellationToken,
        TimeProvider timeProvider,
        IConfiguration config) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess switch
            {
                true => HandleLoginSuccess(context, result.Value, timeProvider, config),
                false => Results.NotFound(result.errorList)
            };

            // return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.errorList);
        });

        group.MapPost("/register", async (RegisterUserCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.errorList);
        });
    }

    private static IResult HandleLoginSuccess(HttpContext context, LoginUserOutput output, TimeProvider timeProvider, IConfiguration config)
    {
        context.Response.Cookies.Append("X-Refresh-Token", output.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = timeProvider.GetUtcNow().AddDays(config.GetValue<double>("RefreshToken:ExpireInDays", 7))
        });

        return Results.Ok(output.JwtToken);
    }
}
