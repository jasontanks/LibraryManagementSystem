using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<AddBookCommandHandler> _logger;

    public AddBookCommandHandler(IBookRepository bookRepository, ILogger<AddBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<BookDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                LibraryId = request.LibraryId
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            _logger.LogInformation("Book with ID {BookId} was created successfully.", book.Id);

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                LibraryId = book.LibraryId
            };
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while creating a book with title '{BookTitle}'.", request.Title);
            throw; // Re-throw the specific DatabaseOperationException to be handled by the middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a new book with title '{BookTitle}'.", request.Title);
            throw; // Re-throw the exception to be handled by the global exception handler middleware
        }
    }
}