using TaskSync.Domain.Excpetions;

namespace TaskSync.Domain.ResultPattern;

public class ResultBase
{
    protected ResultBase(bool isSuccess, List<Error> errorList)
    {
        if ((isSuccess && errorList.Count > 0) || (!isSuccess && errorList.Count == 0))
        {
            throw new InvalidResultPatternException();
        }

        IsSuccess = isSuccess;
        this.errorList = errorList;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public readonly List<Error> errorList;


    public static ResultBase Success() => new(true, []);
    public static ResultBase Failure(List<Error> errors) => new(false, errors);

    public static implicit operator ResultBase(List<Error> errors) => Failure(errors);
    public static implicit operator ResultBase(Error error) => Failure([error]);
}