using System.Security.Cryptography;
using System.Text;
using TaskSync.Application.Interfaces.Security;

namespace TaskSync.Infrastructure.Security;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    public async Task<string> CalculateHash(string plaintextToken)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plaintextToken);
        byte[] hashBytes = SHA512.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public async Task<string> GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[64];
        RandomNumberGenerator.Fill(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<bool> Verify(string plaintextToken, string hash)
    {
        //Generating Hash From Input Token
        byte[] bytes = Encoding.UTF8.GetBytes(plaintextToken);
        byte[] computedHash = SHA512.HashData(bytes);

        //Storing hash converting DB hash from base64 to normal Data
        byte[] storedHash = Convert.FromBase64String(hash);

        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }

}
