using TaskSync.Domain.Entities;

namespace TaskSync.Application.Interfaces.Security;

public interface IJwtProvider
{
    string GenerateToken(User user);

}
