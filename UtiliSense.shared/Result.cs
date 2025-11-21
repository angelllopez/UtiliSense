namespace UtiliSense.api.Core.shared
{
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
        NullOrEmpty // Added to match ErrorMessages.NullOrEmpty
    }
}
