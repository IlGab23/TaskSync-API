using MediatR;
using TaskSync.Application.Features.Security.Commands;
using TaskSync.Application.Interfaces;
using TaskSync.Application.Interfaces.Security;
using TaskSync.Domain.Entities;
using TaskSync.Domain.Entities.ValueObjects;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Security.Handlers;

public class RegistrationHandler(IApplicationDbContext appDbContext, IPasswordHasher passHasher, TimeProvider timeProvider) : IRequestHandler<RegisterUserCommand, ResultBase>
{
    public async Task<ResultBase> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if(appDbContext.Users.Any(u => u.Email.Value == request.Email)) return Error.Validation("Registration.AccountArleadyExists", "The account arleady exists");

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure) return emailResult.errorList;
        
        string hashedPassword = await passHasher.HashAsync(request.Password);

        var newUserResult = User.Create(request.Username, emailResult.Value, hashedPassword, timeProvider.GetUtcNow());
        if (newUserResult.IsFailure) return newUserResult.errorList;
        User newUser = newUserResult.Value;

        await appDbContext.Users.AddAsync(newUser, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }

}
