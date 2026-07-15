using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskSync.Application.Features.BackgroundJob.Commands;
using TaskSync.Application.Interfaces;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.BackgroundJob.Handlers;

public class NotificationPendingTaskDue(IApplicationDbContext appDbContext, TimeProvider timeProvider, ILogger<NotificationPendingTaskDue> log) : IRequestHandler<PendingTaskDueCommand, ResultBase>
{
    private record TaskNotificationData(string Title, Guid UserId);

    public async Task<ResultBase> Handle(PendingTaskDueCommand request, CancellationToken cancellationToken)
    {
        List<TaskNotificationData> DueTasks = await appDbContext.TaskSyncs.AsNoTracking()
                                    .Where(t => t.DueDateUtc.Value <= timeProvider.GetUtcNow() && !t.IsCompleted)
                                    .Select(t => new TaskNotificationData(
                                        t.Title,
                                        t.UserId
                                    )).ToListAsync(cancellationToken);

        if(DueTasks.Count == 0) return ResultBase.Success();

        foreach (var task in DueTasks)
        {
            if (log.IsEnabled(LogLevel.Critical))
            {
                log.LogCritical("Task '{TaskTitle}' for User '{TaskUserId}' is due!", task.Title, task.UserId);
            }
        }

        return ResultBase.Success();
    }
}
