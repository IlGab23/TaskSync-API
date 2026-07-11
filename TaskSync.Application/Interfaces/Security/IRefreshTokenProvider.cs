namespace TaskSync.Application.Interfaces.Security;

public interface IRefreshTokenProvider
{
    Task<string> GenerateRefreshToken();
    Task<string> CalculateHash(string plaintextToken);
    Task<bool> Verify(string plaintextToken, string hash);
}
