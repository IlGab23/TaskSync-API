namespace TaskSync.Domain.ResultPattern;

public record Error (string Name, string Description, string? Details = null)
{
    public static readonly Error none = new(string.Empty, string.Empty);
}

