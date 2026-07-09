using Microsoft.EntityFrameworkCore;
using TaskSync.Infrastructure.Persistence;

namespace TaskSync.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IServiceProvider sp)
    {
        using var scope = sp.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
