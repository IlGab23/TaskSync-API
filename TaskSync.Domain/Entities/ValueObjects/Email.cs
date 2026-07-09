using System.Text.RegularExpressions;
using TaskSync.Domain.ResultPattern;
namespace TaskSync.Domain.Entities.ValueObjects;

public sealed partial record Email
{
    private const int MaxLenght = 254;

    public string Value { get; }

    private Email(string value) => Value = value;

    #pragma warning disable CS8618
    private Email() {} //Costruttore vuoto Per EF Core

    public static Result<Email> Create(string email)
    {
        List<Error> errors = new();
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(Error.Validation("Email.Empty", "Email field is empty"));
        }

        if (email.Length > MaxLenght)
        {
            errors.Add(Error.Validation("Email.MaxLeghtLimitExceeded", "Email is too long, the max lenght is 254 characters"));
        }

        if (!EmailRegex().IsMatch(email))
        {
            errors.Add(Error.Validation("Email.InvalidEmailFormat", "Email format is not valid"));
        }

        return errors.Count > 0 ? errors : new Email(email.Trim().ToLowerInvariant());

        // return new Email(email.Trim().ToLowerInvariant());
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();
}