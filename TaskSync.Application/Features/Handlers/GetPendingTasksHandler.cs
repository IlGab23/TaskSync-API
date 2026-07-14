using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskSync.Application.Features.Querys;
using TaskSync.Application.Interfaces;
using TaskSync.Domain.Entities;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Handlers;

public class GetPendingTasksHandler(IApplicationDbContext appDbContext) : IRequestHandler<GetPendingTasksQuery, Result<GetPendingTasksQueryOutput>>
{
    public async Task<Result<GetPendingTasksQueryOutput>> Handle(GetPendingTasksQuery request, CancellationToken cancellationToken)
    {
        List<TaskData> taskList = await appDbContext.TaskSyncs.AsNoTracking()
                                        .Where(t => t.UserId == request.UserId && !t.IsCompleted)
                                        .Select(t => new TaskData
                                        (
                                            t.Title,
                                            t.Description,
                                            t.DueDateUtc.Value,
                                            t.IsCompleted,
                                            t.CreatedAtUtc
                                        )).ToListAsync(cancellationToken);

        return new GetPendingTasksQueryOutput(taskList);
    }

}
