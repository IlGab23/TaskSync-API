using Microsoft.EntityFrameworkCore;
using TaskSync.Domain.Entities;
using TaskSync.Domain.Entities.SecurityEntities;

namespace TaskSync.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<SyncTask> TaskSyncs { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
