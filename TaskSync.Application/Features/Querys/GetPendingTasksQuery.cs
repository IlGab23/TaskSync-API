using MediatR;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TaskSync.Domain.Entities.ValueObjects;
using TaskSync.Domain.ResultPattern;
namespace TaskSync.Application.Features.Querys;

public record GetPendingTasksQuery(Guid UserId) : IRequest<Result<GetPendingTasksQuery>>;
public record GetPendingTasksQueryOutput(List<Task> Tasks);
public record Task(string Title, string Description, DateTimeOffset Expiry, bool IsCompleted, DateTimeOffset CreatedAtUtc);