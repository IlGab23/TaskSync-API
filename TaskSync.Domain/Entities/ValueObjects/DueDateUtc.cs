using TaskSync.Domain.ResultPattern;
namespace TaskSync.Domain.Entities.ValueObjects;

public record DueDateUtc
{
    public DateTimeOffset Value { get; }

    private DueDateUtc(DateTimeOffset date) => Value = date;

    #pragma warning disable CS8618
    private DueDateUtc() {} //Costruttore vuoto Per EF Core

    public static Result<DueDateUtc> Create(DateTimeOffset date, TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        List<Error> errors = new();
        if (date == default)
        {
            errors.Add(Error.Validation("DueDateUtc.EmptyDate", "Due Date is empty or null"));
        }

        if (date < now)
        {
            errors.Add(Error.Validation("DueDateUtc.PastDate", "Due Date cannot be in the past"));
        }

        if (date > now.AddYears(1))
        {
            errors.Add(Error.Validation("DueDateUtc.TooFar", "Due Date cannot be set more than 1 year in the future"));
        }

        return errors.Count > 0 ? errors : new DueDateUtc(date);
    }
}
