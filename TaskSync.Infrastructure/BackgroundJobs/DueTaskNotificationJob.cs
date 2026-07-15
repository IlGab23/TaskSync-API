using MediatR;
using Quartz;
using TaskSync.Application.Features.BackgroundJob.Commands;

namespace TaskSync.Infrastructure.BackgroundJobs;

public class DueTaskNotificationJob : IJob
{
    private readonly ISender _sender;

    
    public DueTaskNotificationJob(ISender sender)
    {
        _sender = sender;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _ = await _sender.Send(new PendingTaskDueCommand(), context.CancellationToken);
    }
}
