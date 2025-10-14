using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetBooksByLibraryQuery : IRequest<IEnumerable<BookDto>>
{
    public Guid LibraryId { get; set; }

    public GetBooksByLibraryQuery(Guid libraryId)
    {
        LibraryId = libraryId;
    }
}