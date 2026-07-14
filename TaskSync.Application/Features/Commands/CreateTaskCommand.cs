using FluentValidation;
using MediatR;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Commands;

public record CreateTaskCommand(Guid UserId, string Title, string Description, DateTimeOffset Expiry) : IRequest<Result<CreateTaskCommandOutput>>;
public record CreateTaskCommandOutput(Guid TaskId);

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Task title cannot be empty")
            .Length(10, 50).WithMessage("Task title must be in range of 10 to 50 characters")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Task title can only have letters and numbers");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Task description cannot be empty")
            .Length(20, 200).WithMessage("Task title must be in range of 10 to 50 characters");

        RuleFor(c => c.Expiry)
            .NotNull().WithMessage("Task must have an expiry date");

    }
}
