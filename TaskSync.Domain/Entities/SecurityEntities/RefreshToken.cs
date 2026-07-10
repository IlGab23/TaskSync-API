namespace TaskSync.Domain.Entities.SecurityEntities;

public class RefreshToken
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }

    public string TokenHash { get; init; } = string.Empty;
    public DateTimeOffset Expiry { get; init; }
    public DateTimeOffset? RevokedAt { get; private set; }

    public string? ReplacedByTokenHash { get; private set; }

    public bool IsExpired(TimeProvider timeProvider) => timeProvider.GetUtcNow() >= Expiry;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive(TimeProvider timeProvider) => !IsExpired(timeProvider) && !IsRevoked;
    public void Revoke(DateTimeOffset RevokeDate, string? ReplacedBy = null)
    {
        RevokedAt = RevokeDate;
        ReplacedByTokenHash = ReplacedBy;
    }
}
