namespace SharedKernel.Errors;

public record Error(string Code, string Description, ErrorType ErrorType)
{
    public static Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);

    public static Error Failure(string code, string description)
        => new(code, description, ErrorType.Failure);

    public static Error Validation(string code, string description)
        => new(code, description, ErrorType.Validation);

    public static Error NotFound(string code, string description) 
        => new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description)
        => new(code, description, ErrorType.Conflict);

    public override string ToString()
        => $"{Code}, {Description}";
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}