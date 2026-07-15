using MediatR;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.BackgroundJob.Commands;

public record PendingTaskDueCommand() : IRequest<ResultBase>;
