// Could have been records, but we're going to make it interesting..

public abstract class Result<T>(T value)
{
    public T Value { get; } = value;
}

public class Failure<T>(T value, IEnumerable<string> errors)
    : Result<T>(value)
{
    public IEnumerable<string> Errors { get; } = errors;
}

public class Success<T>(T value) : Result<T>(value);