using Microsoft.EntityFrameworkCore;
using TaskSync.Domain.Entities;

namespace TaskSync.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<SyncTask> TaskSyncs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
