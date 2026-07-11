namespace TaskSync.Domain.ResultPattern;

public record Error (string Name, string Description, string? Details = null)
{
    public static readonly Error none = new(string.Empty, string.Empty);
    public static Error Validation(string Name, string Description, string? Details = null) => new(Name, Description, Details);
    public static readonly Error Login = new("Login.InvalidCredentials", "Email or Password are incorrect");
}

