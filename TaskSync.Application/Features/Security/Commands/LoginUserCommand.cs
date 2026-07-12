using System.Data;
using FluentValidation;
using MediatR;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Security.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserOutput>>; // Input
public record LoginUserOutput(string JwtToken, string RefreshToken); //Output

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid format of email");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}

