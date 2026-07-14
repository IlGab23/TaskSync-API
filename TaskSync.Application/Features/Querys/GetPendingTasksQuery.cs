using MediatR;
using Microsoft.VisualBasic;
using TaskSync.Domain.ResultPattern;
namespace TaskSync.Application.Features.Querys;

public record GetPendingTasksQuery(Guid UserId) : IRequest<Result<GetPendingTasksQuery>>;
public record GetPendingTasksQueryOutput(List<Task> Tasks);
public record Task(string Title, string Description, DueDate Expiry, bool IsCompleted, DateTimeOffset CreatedAtUtc);