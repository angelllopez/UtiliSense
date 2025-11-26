namespace UtiliSense.shared;

/// <summary>
/// Contains standardized external error messages intended for API responses.
/// Intended to be user-friendly and non-technical.
/// </summary>
public static class ExternalErrorMessages
{
    /// <summary>
    /// Gets the error message for a not found resource.
    /// </summary>
    public static string NotFound => "The requested resource was not found.";

    /// <summary>
    /// Gets the error message for an invalid request.
    /// </summary>
    public static string InvalidRequest => "The request is invalid.";

    /// <summary>
    /// Gets the error message for an unauthorized access attempt.
    /// </summary>
    public static string Unauthorized => "You are not authorized to access this resource.";

    /// <summary>
    /// Gets the error message for a forbidden access attempt.
    /// </summary>
    public static string Forbidden => "You do not have permission to access this resource.";

    /// <summary>
    /// Gets the error message for a conflict.
    /// </summary>
    public static string Conflict => "There is a conflict with the current state of the resource.";

    /// <summary>
    /// Gets the error message for a server error.
    /// </summary>
    public static string ServerError => "An unexpected error occurred on the server.";
}
