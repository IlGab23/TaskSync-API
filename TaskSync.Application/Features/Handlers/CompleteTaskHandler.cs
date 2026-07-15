using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskSync.Application.Features.Commands;
using TaskSync.Application.Interfaces;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Handlers;

public class CompleteTaskHandler(IApplicationDbContext appDbContext) : IRequestHandler<CompleteTaskCommand, Result<CompleteTaskCommandOutput>>
{
    public async Task<Result<CompleteTaskCommandOutput>> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        if (!await appDbContext.TaskSyncs.AnyAsync(t => t.Id == request.TaskId && t.UserId == request.UserId, cancellationToken)) return Error.Validation("SyncTask.NotFound", "Task doesn't exists");

        await appDbContext.TaskSyncs.Where(t => t.Id == request.TaskId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(t => t.IsCompleted, true), cancellationToken);

        return new CompleteTaskCommandOutput(request.TaskId);
    }

}
