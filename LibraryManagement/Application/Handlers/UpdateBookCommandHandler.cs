using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<UpdateBookCommandHandler> _logger;

    public UpdateBookCommandHandler(IBookRepository bookRepository, ILogger<UpdateBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book is null)
            {
                return false;
            }

            book.Title = request.Title;
            var result = await _bookRepository.SaveChangesAsync();
            if (result)
            {
                _logger.LogInformation("Book with ID {BookId} was updated successfully.", request.Id);
            }
            return result;
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while updating a book with ID '{BookId}'.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating a book with ID '{BookId}'.", request.Id);
            throw;
        }
    }
}