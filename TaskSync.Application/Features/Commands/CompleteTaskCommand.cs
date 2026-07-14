using System.Data;
using FluentValidation;
using MediatR;
using TaskSync.Domain.ResultPattern;

namespace TaskSync.Application.Features.Commands;

public record CompleteTaskCommand(Guid UserId, Guid TaskId) : IRequest<Result<CompleteTaskCommandOutput>>;
public record CompleteTaskCommandOutput(Guid TaskId, bool IsSuccess);

public class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
{
    public CompleteTaskCommandValidator()
    {
        RuleFor(c => c.TaskId)
            .NotEmpty().WithMessage("Task Id cannot be empty");
    }
}
