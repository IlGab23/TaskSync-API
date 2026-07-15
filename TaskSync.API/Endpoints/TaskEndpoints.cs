using System.Security.Claims;
using MediatR;
using TaskSync.Application.Features.Commands;
using TaskSync.Application.Features.Querys;
using TaskSync.Domain.ResultPattern;
using static System.Guid;

namespace TaskSync.API.Endpoints;

public static class TaskEndpoints
{
    public record CreateTaskRequest(string Title, string Description, DateTimeOffset Expiry);
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Task Endpoints");

        group.MapPost("/", async (CreateTaskRequest request,
        ISender sender,
        ClaimsPrincipal user,
        CancellationToken cancellationToken) =>
        {
            Guid parsedUserId = GetUserId(user);
            if (parsedUserId == Guid.Empty) return Results.Unauthorized();

            var finalCommand = new CreateTaskCommand(parsedUserId, request.Title, request.Description, request.Expiry);

            var result = await sender.Send(finalCommand, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.errorList);
        }).RequireAuthorization();

        group.MapGet("/", async (ISender sender, ClaimsPrincipal user, CancellationToken cancellationToken) =>
        {
            Guid parsedUserId = GetUserId(user);
            if (parsedUserId == Guid.Empty) return Results.Unauthorized();

            var query = new GetPendingTasksQuery(parsedUserId);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.errorList);
        }).RequireAuthorization();

        group.MapPatch("/{taskId}/complete", async (Guid taskId,
        ISender sender,
        ClaimsPrincipal user,
        CancellationToken cancellationToken) =>
        {
            Guid parsedUserId = GetUserId(user);
            if (parsedUserId == Guid.Empty) return Results.Unauthorized();

            var command = new CompleteTaskCommand(parsedUserId, taskId);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.errorList);
        }).RequireAuthorization();

    }

    private static Guid GetUserId(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdString)) return Guid.Empty;

        if (!Guid.TryParse(userIdString, out Guid parsedUserId)) return Guid.Empty;

        return parsedUserId;
    }
}

