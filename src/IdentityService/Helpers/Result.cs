using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Helpers;

public class Result<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]

    public bool IsSuccess { get; }

    public T? Value { get; }
    public string? Error { get; }

    public T? ErrorValue { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result()
    {
        IsSuccess = true;
        Value = default;
    }

    private Result(string? error)
    {
        IsSuccess = false;
        Error = error;
    }

    private Result(string? error, T? value)
    {
        IsSuccess = false;
        ErrorValue = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Success() => new();
    public static Result<T> Failure(string? error) => new(error);
    public static Result<T> Failure(string? error, T errorValue) => new(error, errorValue);
}