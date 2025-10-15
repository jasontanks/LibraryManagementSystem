using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetBooksByLibraryQueryHandler : IRequestHandler<GetBooksByLibraryQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<GetBooksByLibraryQueryHandler> _logger;

    public GetBooksByLibraryQueryHandler(IBookRepository bookRepository, ILogger<GetBooksByLibraryQueryHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBooksByLibraryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var books = await _bookRepository.GetByLibraryIdAsync(request.LibraryId);

            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                LibraryId = b.LibraryId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving books for library ID {LibraryId}.", request.LibraryId);
            throw;
        }
    }
}