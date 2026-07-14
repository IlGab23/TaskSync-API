using MediatR;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TaskSync.Domain.Entities.ValueObjects;
using TaskSync.Domain.ResultPattern;
namespace TaskSync.Application.Features.Querys;

public record GetPendingTasksQuery(Guid UserId) : IRequest<Result<GetPendingTasksQuery>>;
public record GetPendingTasksQueryOutput(List<TaskData> Tasks);
public record TaskData(string Title, string Description, DateTimeOffset Expiry, bool IsCompleted, DateTimeOffset CreatedAtUtc);