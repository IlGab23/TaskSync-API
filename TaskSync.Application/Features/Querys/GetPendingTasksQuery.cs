using MediatR;
using TaskSync.Domain.ResultPattern;
namespace TaskSync.Application.Features.Querys;

public record GetPendingTasksQuery(Guid UserId) : IRequest<Result<GetPendingTasksQueryOutput>>;
public record GetPendingTasksQueryOutput(List<TaskData> Tasks);
public record TaskData(string Title, string Description, DateTimeOffset Expiry, bool IsCompleted, DateTimeOffset CreatedAtUtc);