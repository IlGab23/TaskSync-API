namespace TaskSync.Domain.Excpetions;

public class InvalidResultPatternException : Exception
{
    public InvalidResultPatternException() : base("Invalid Result Pattern") {}
    public InvalidResultPatternException(string message) : base(message) {}
    public InvalidResultPatternException(string message, Exception innerException) : base(message, innerException) {}
}
