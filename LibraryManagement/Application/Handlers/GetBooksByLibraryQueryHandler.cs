using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetBooksByLibraryQueryHandler : IRequestHandler<GetBooksByLibraryQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksByLibraryQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBooksByLibraryQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetByLibraryIdAsync(request.LibraryId);

        return books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            LibraryId = b.LibraryId
        });
    }
}