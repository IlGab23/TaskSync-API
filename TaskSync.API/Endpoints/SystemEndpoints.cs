namespace TaskSync.API.Endpoints;

public static class SystemEndpoints
{
    public static void MapSystemEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/system").WithTags("System Endpoints");

        group.MapGet("/ping", async () => Results.Ok("pong!"));
    }
}
