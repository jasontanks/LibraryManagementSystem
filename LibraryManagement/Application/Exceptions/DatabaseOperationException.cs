using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Application.Exceptions;

public class DatabaseOperationException : DomainException
{
    public DatabaseOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}