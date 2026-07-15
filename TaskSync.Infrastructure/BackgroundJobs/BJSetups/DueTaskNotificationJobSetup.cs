using Microsoft.Extensions.Options;
using Quartz;

namespace TaskSync.Infrastructure.BackgroundJobs.BJSetups;

public class DueTaskNotificationJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(DueTaskNotificationJob));

        options.AddJob<DueTaskNotificationJob>(jobBuilder => jobBuilder
            .WithIdentity(jobKey)
            .WithDescription("Background job that checks for due tasks and sends notifications to users.")
        );

        options.AddTrigger(triggerBuilder => triggerBuilder
            .ForJob(jobKey)
            .WithIdentity("NotificationDueTask-Trigger")
            .WithDescription("Trigger that fires every minute to execute the due task notification job.")
            .WithCronSchedule("0 * * * * ? *", cronBuilder => cronBuilder
                .InTimeZone(TimeZoneInfo.Utc)
                .WithMisfireHandlingInstructionDoNothing()
                )
        );

    }

}
