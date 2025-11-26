namespace UtiliSense.api.Core.shared;

/// <summary>
/// Represents the result of an operation, containing either a value of type <typeparamref name="T"/> on success or
/// error information on failure.
/// </summary>
/// <remarks>Use <see cref="Success(T)"/> to create a successful result and <see cref="Failure(ErrorCode,
/// string)"/> to create a failed result. The <see cref="IsSuccess"/> property indicates whether the operation was
/// successful. When <see cref="IsSuccess"/> is <see langword="true"/>, <see cref="Data"/> contains the result value;
/// otherwise, <see cref="ErrorCode"/> and <see cref="ErrorMessage"/> provide error details.</remarks>
/// <typeparam name="T">The type of the value returned when the operation succeeds.</typeparam>
public class Result<T>
{
    public T? Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public ErrorCode? ErrorCode { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static Result<T> Success(T data) => new() { Data = data, IsSuccess = true };
    public static Result<T> Failure(ErrorCode errorCode, string message) => new() { IsSuccess = false, ErrorCode = errorCode, ErrorMessage = message };
}

public enum ErrorCode
{
    None,
    NotFound,
    OutOfRange,
    ValidationError,
    Unauthorized,
    Forbidden,
    Conflict,
    InternalServerError,
    InvalidInput,
    NullOrEmpty // Added to match InternalErrorMessages.NullOrEmpty
}
