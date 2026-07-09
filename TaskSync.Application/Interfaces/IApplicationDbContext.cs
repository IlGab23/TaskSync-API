using Microsoft.EntityFrameworkCore;
using TaskSync.Domain.Entities;

namespace TaskSync.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> User { get; }
    DbSet<SyncTask> TaskSync { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
