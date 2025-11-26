using FluentValidation.Results;

namespace UtiliSense.shared;

/// <summary>
/// Contains standardized internal log messages for diagnostics and debugging.
/// Not intended for public/API responses.
/// Ensure that no sensitive information is passed to these methods.
/// </summary>
public static class InternalErrorMessages
{
    /// <summary>
    /// Generates a formatted message indicating that validation has failed for a specified entity.
    /// </summary>
    /// <param name="entity">The name of the entity for which validation failed. Cannot be null or empty.</param>
    /// <param name="details">A list of <see cref="ValidationFailure"/> objects containing details about the validation errors. Cannot be
    /// null.</param>
    /// <returns>A formatted string describing the validation failure, including the entity name and details of the errors.</returns>
    public static string ValidationFailed(string entity, List<ValidationFailure> details) =>
        $"Validation failed for '{entity}'. Details: {details.ToList()}";

    /// <summary>
    /// Generates an error message indicating that a specified property has an invalid value.
    /// </summary>
    /// <param name="property">The name of the property with the invalid value.</param>
    /// <param name="value">The invalid value associated with the property. Can be <see langword="null"/>.</param>
    /// <returns>A formatted error message specifying the property and its invalid value.</returns>
    public static string InvalidValue(string property, object? value) =>
        $"Invalid value '{value}' for property '{property}'.";

    /// <summary>
    /// Generates a message indicating that a value for a specified property is out of the expected range.
    /// </summary>
    /// <param name="property">The name of the property associated with the out-of-range value.</param>
    /// <param name="value">The actual value that is out of range. Can be <see langword="null"/>.</param>
    /// <param name="min">The minimum allowable value for the property. Can be <see langword="null"/>.</param>
    /// <param name="max">The maximum allowable value for the property. Can be <see langword="null"/>.</param>
    /// <returns>A formatted string describing the out-of-range condition, including the property name, actual value, and
    /// expected range.</returns>
    public static string OutOfRangeValue(string property, object? value, object? min, object? max) =>
        $"Value '{value}' for property '{property}' is out of range. Expected between '{min}' and '{max}'.";

    /// <summary>
    /// Generates a message indicating that an operation has failed, including the reason for the failure.
    /// </summary>
    /// <param name="operation">The name or description of the operation that failed.</param>
    /// <param name="reason">The reason for the operation's failure.</param>
    /// <returns>A formatted string describing the failed operation and the reason for the failure.</returns>
    public static string OperationFailed(string operation, string reason) =>
        $"Operation '{operation}' failed. Reason: {reason}";

    /// <summary>
    /// Formats a message describing an exception that occurred in a specified method.
    /// </summary>
    /// <param name="method">The name of the method where the exception occurred.</param>
    /// <param name="ex">The exception instance that was thrown.</param>
    /// <returns>A formatted string containing the method name, exception type, and exception message.</returns>
    public static string ExceptionOccurred(string method, Exception ex) =>
        $"Exception in '{method}': {ex.GetType().Name} - {ex.Message}";

    /// <summary>
    /// Generates an error message indicating that a specified parameter is null or empty.
    /// </summary>
    /// <param name="paramName">The name of the parameter that is null or empty.</param>
    /// <returns>A string containing the error message for the null or empty parameter.</returns>
    public static string NullOrEmptyParameter(string paramName) =>
        $"Parameter '{paramName}' is null or empty.";

    /// <summary>
    /// Generates a standardized error message indicating that a record of the specified type  with the given
    /// identifier was not found.
    /// </summary>
    /// <param name="recordType">The type of the record that was not found (e.g., "User", "Order").</param>
    /// <param name="identifier">The unique identifier of the record that was not found.</param>
    /// <returns>A formatted error message indicating the missing record.</returns>
    public static string RecordNotFound(string recordType, string identifier) =>
        $"{recordType} with identifier '{identifier}' was not found.";

    /// <summary>
    /// Generates a conflict message indicating that a record of the specified type with the given identifier
    /// already exists.
    /// </summary>
    /// <param name="recordType">The type of the record that caused the conflict (e.g., "User", "Order").</param>
    /// <param name="identifier">The unique identifier of the record that caused the conflict.</param>
    /// <returns>A string message indicating the conflict, including the record type and identifier.</returns>
    public static string RecordConflict(string recordType, string identifier) =>
        $"{recordType} with identifier '{identifier}' already exists.";

    /// <summary>
    /// Generates a detailed error message for an AutoMapper mapping failure.
    /// </summary>
    /// <param name="sourceType">The name of the source type involved in the mapping operation.</param>
    /// <param name="destType">The name of the destination type involved in the mapping operation.</param>
    /// <param name="details">Additional details about the mapping failure.</param>
    /// <returns>A formatted error message describing the mapping failure, including the source type, destination type, and
    /// additional details.</returns>
    public static string AutomapperMappingException(string sourceType, string destType, string details) =>
        $"Automapper failed to map from '{sourceType}' to '{destType}'. Details: {details}";

    /// <summary>
    /// Generates an error message indicating that the specified argument cannot be null.
    /// </summary>
    /// <param name="paramName">The name of the parameter that is null.</param>
    /// <returns>A string containing the error message.</returns>
    public static string ArgumentNullException(string paramName) =>
        $"Argument '{paramName}' cannot be null.";
}
