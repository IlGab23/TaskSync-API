using TaskSync.Domain.Entities.SecurityEntities;
using TaskSync.Domain.Entities.ValueObjects;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Domain.Entities;

public sealed class User
{
    private readonly List<SyncTask> _syncTasks = new();
    private readonly List<RefreshToken> _refreshTokens = new();

    public Guid Id { get; }
    public string Username { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; }
    public IReadOnlyCollection<SyncTask> SyncTasks => _syncTasks.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User(Guid id, string username, Email email, string passwordHash, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAtUtc = createdAtUtc;
    }

    #pragma warning disable CS8618
    private User() {} //Costrutto vuoto per EF Core

    public static Result<User> Create(string username, Email email, string passwordHash, DateTimeOffset createdAtUtc)
    {
        List<Error> errors = new();
        if (string.IsNullOrWhiteSpace(username))
        {
            errors.Add(Error.Validation("User.InvalidUserName", "The Username is empty or null"));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            errors.Add(Error.Validation("User.InvalidPassword", "User password is empty or null"));
        }

        return errors.Count > 0 ? errors : new User(Guid.NewGuid(), username, email, passwordHash, createdAtUtc);

    }
}
