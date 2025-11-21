using FluentValidation.Results;

namespace UtiliSense.shared
{
    /// <summary>
    /// Contains standardized internal log messages for diagnostics and debugging.
    /// Not intended for public/API responses.
    /// Ensure that no sensitive information is passed to these methods.
    /// </summary>
    public static class ErrorMessages
    {
        public static string ValidationFailed(string entity, List<ValidationFailure> details) =>
            $"Validation failed for '{entity}'. Details: {details.ToList()}";

        public static string InvalidValue(string property, object? value) =>
            $"Invalid value '{value}' for property '{property}'.";

        public static string OutOfRangeValue(string property, object? value, object? min, object? max) =>
            $"Value '{value}' for property '{property}' is out of range. Expected between '{min}' and '{max}'.";

        public static string OperationFailed(string operation, string reason) =>
            $"Operation '{operation}' failed. Reason: {reason}";

        public static string ExceptionOccurred(string method, Exception ex) =>
            $"Exception in '{method}': {ex.GetType().Name} - {ex.Message}";

        public static string NullOrEmptyParameter(string paramName) =>
            $"Parameter '{paramName}' is null or empty.";

        public static string RecordNotFound(string recordType, string identifier) =>
            $"{recordType} with identifier '{identifier}' was not found.";

        public static string RecordConflict(string recordType, string identifier) =>
            $"{recordType} with identifier '{identifier}' already exists.";

        public static string AutomapperMappingException(string sourceType, string destType, string details) =>
            $"Automapper failed to map from '{sourceType}' to '{destType}'. Details: {details}";

        public static string ArgumentNullException(string paramName) =>
            $"Argument '{paramName}' cannot be null.";
    }
}
