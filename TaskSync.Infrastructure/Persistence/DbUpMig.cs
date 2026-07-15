using System.Reflection;
using DbUp;
using Microsoft.AspNetCore.Builder;

namespace TaskSync.Infrastructure.Persistence;

public static class DbUpMig
{
    public static IApplicationBuilder UseDbUpMigrations(this IApplicationBuilder app, string connectionString)
    {
        if(string.IsNullOrWhiteSpace(connectionString)) throw new Exception("Quartz connection string is null or empty");

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fatal Error in DbUp: {result.Error}");
            Console.ResetColor();

            throw new Exception("DbUp Migration failed!", result.Error);
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("DbUp Migration success");
        Console.ResetColor();

        return app;
    }
}
