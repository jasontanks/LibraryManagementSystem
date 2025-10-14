using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetAllBooksQuery : IRequest<PaginatedList<BookDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}