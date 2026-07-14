using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskSync.Application.Features.Commands;
using TaskSync.Application.Interfaces;
using TaskSync.Domain.Entities;
using TaskSync.Domain.Entities.ValueObjects;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Handlers;

public class CreateTaskHandler(IApplicationDbContext appDbContext, TimeProvider timeProvider) : IRequestHandler<CreateTaskCommand, Result<CreateTaskCommandOutput>>
{
    public async Task<Result<CreateTaskCommandOutput>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        if (await appDbContext.TaskSyncs.AnyAsync(t => t.Title == request.Title && t.UserId == request.UserId, cancellationToken))
        {
            return Error.Validation("SyncTask.AlreadyExists", "Already exists a task under this user");
        }

        var expiryDateResult = DueDateUtc.Create(request.Expiry, timeProvider);
        if (expiryDateResult.IsFailure) return expiryDateResult.errorList;

        DateTimeOffset nowDate = timeProvider.GetUtcNow();
        var syncTaskResult = SyncTask.Create(request.Title, request.Description, expiryDateResult.Value, request.UserId, nowDate);
        if (syncTaskResult.IsFailure) return syncTaskResult.errorList;

        await appDbContext.TaskSyncs.AddAsync(syncTaskResult.Value, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new CreateTaskCommandOutput(syncTaskResult.Value.Id);
    }

}
