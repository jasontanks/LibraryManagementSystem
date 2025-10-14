using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PaginatedList<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<PaginatedList<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _bookRepository.GetAllAsync(request.PageNumber, request.PageSize);
        var bookDtos = items.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            LibraryId = b.LibraryId
        }).ToList();

        return new PaginatedList<BookDto>(bookDtos, totalCount, request.PageNumber, request.PageSize);
    }
}