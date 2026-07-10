using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using TaskSync.Application.Interfaces.Security;

namespace TaskSync.Infrastructure.Security;

public class Argon2idPasswordHasher : IPasswordHasher
{
    private static byte[] GenerateSalt()
    {
        var buffer = new byte[16];
        RandomNumberGenerator.Fill(buffer);
        return buffer;
    }

    private static async Task<byte[]> GenerateHashAsync(byte[] salt, string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        using var argon2 = new Argon2id(passwordBytes)
        {
            Salt = salt,
            DegreeOfParallelism = 1,
            MemorySize = 1024 * 64,
            Iterations = 3
        };

        return await argon2.GetBytesAsync(32);
    }

    public async Task<string> HashAsync(string password)
    {
        byte[] salt = GenerateSalt();

        byte[] hash = await GenerateHashAsync(salt, password);

        return $"{Convert.ToBase64String(salt)}-{Convert.ToBase64String(hash)}";
    }

    public async Task<bool> Verify(string password, string hash)
    {
        string[] splittedHash = hash.Split('-');
        if (splittedHash.Length != 2)
        {
            return false;
        }

        byte[] salt = Convert.FromBase64String(splittedHash[0]);
        byte[] storedHash = Convert.FromBase64String(splittedHash[1]);

        byte[] computedHash = await GenerateHashAsync(salt, password);

        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}
