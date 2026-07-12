using FluentValidation;
using MediatR;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Security.Commands;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<ResultBase>; //Input
// public record RegisterUserOutput(bool IsRegisteredOperetionDone); //Output

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("Username cannot be empty")
            .Length(3, 100).WithMessage("Username must be in range of 3 and 100 of length")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Username can only accept letters and numbers");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid format of email")
            .MaximumLength(254);

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(50).WithMessage("Password must be smaller than 51 characters")
            .Matches("[a-z]").WithMessage("Password must contain at least 1 lower letter")
            .Matches("[A-Z]").WithMessage("Password must contain at least 1 upper letter")
            .Matches("[0-9]").WithMessage("Password must contain at least 1 number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least 1 special character");
    }
}
