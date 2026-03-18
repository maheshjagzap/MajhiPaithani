namespace MajhiPaithani.Domain.Exceptions;

// Base Exception for all application-specific errors
public class AppException : Exception
{
    public int StatusCode { get; }
    public AppException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }
}

// 404 Not Found
public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404) { }
}

// 401 Unauthorized
public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message, 401) { }
}

// 400 Bad Request
public class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message, 400) { }
}

// 403 Forbidden (User is logged in but lacks permission)
public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message, 403) { }
}

// 409 Conflict (e.g., Email already exists)
public class ConflictException : AppException
{
    public ConflictException(string message) : base(message, 409) { }
}

// 422 Unprocessable Entity (e.g., validation failures)
public class ValidationException : AppException
{
    public ValidationException(string message) : base(message, 422) { }
}

// 423 Locked (e.g., inactive/suspended account or seller)
public class InactiveAccountException : AppException
{
    public InactiveAccountException(string message) : base(message, 423) { }
}