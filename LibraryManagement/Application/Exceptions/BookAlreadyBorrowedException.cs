namespace LibraryManagement.Domain.Exceptions;

public class BookAlreadyBorrowedException : DomainException
{
    public BookAlreadyBorrowedException(Guid bookId)
        : base($"The book with ID '{bookId}' is already borrowed.")
    {
    }
}