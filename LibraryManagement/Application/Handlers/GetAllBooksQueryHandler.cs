using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PaginatedList<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<GetAllBooksQueryHandler> _logger;

    public GetAllBooksQueryHandler(IBookRepository bookRepository, ILogger<GetAllBooksQueryHandler> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<PaginatedList<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var (items, totalCount) = await _bookRepository.GetAllAsync(request.PageNumber, request.PageSize);
            var bookDtos = items.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                LibraryId = b.LibraryId
            }).ToList();

            _logger.LogInformation("Successfully retrieved page {PageNumber} with {BookCount} books.", request.PageNumber, bookDtos.Count);

            return new PaginatedList<BookDto>(bookDtos, totalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving all books.");
            throw;
        }
    }
}