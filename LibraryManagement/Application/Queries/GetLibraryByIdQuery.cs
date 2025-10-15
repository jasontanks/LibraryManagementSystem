using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetLibraryByIdQuery : IRequest<LibraryDto>
{
    public Guid Id { get; set; }

    public GetLibraryByIdQuery(Guid id) => Id = id;
}