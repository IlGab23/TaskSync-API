using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskSync.Application.Features.Security.Commands;
using TaskSync.Application.Interfaces;
using TaskSync.Application.Interfaces.Security;
using TaskSync.Domain.Entities;
using TaskSync.Domain.Entities.SecurityEntities;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Security.Handlers;

public class LoginHandler(IApplicationDbContext appDbContext, IPasswordHasher passHasher, IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider, TimeProvider timeProvider) : IRequestHandler<LoginUserCommand, Result<LoginUserOutput>>
{
    public async Task<Result<LoginUserOutput>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // List<Error> errors = new();

        User? user = await appDbContext.Users.SingleOrDefaultAsync(u => u.Email.Value == request.Email, cancellationToken);

        if (user is null)
        {
            return Error.Login;
        }

        if (!await passHasher.Verify(request.Password, user.PasswordHash))
        {
            return Error.Login;
        }

        string aToken = jwtProvider.GenerateToken(user);

        string rToken = await refreshTokenProvider.GenerateRefreshToken();
        string hashedRefreshToken = await refreshTokenProvider.CalculateHash(rToken);

        RefreshToken rt = RefreshToken.Create(user.Id, hashedRefreshToken, timeProvider.GetUtcNow()).Value;

        await appDbContext.RefreshTokens.AddAsync(rt, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new LoginUserOutput(aToken, rToken);
    }

}
