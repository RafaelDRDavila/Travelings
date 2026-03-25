namespace AuthHexagonalApi.Domain.Exceptions;

public class ValidationException : DomainException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(
        string message,
        IDictionary<string, string[]> errors,
        string? errorCode = null)
        : base(message, errorCode)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}

