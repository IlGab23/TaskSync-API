namespace TaskSync.Application.Interfaces.Security;

public interface IPasswordHasher
{
    Task<string> HashAsync(string password);
    Task<bool> Verify(string password, string hash);

}
