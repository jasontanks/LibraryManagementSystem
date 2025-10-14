using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetLibraryByIdQueryHandler : IRequestHandler<GetLibraryByIdQuery, LibraryDto>
{
    private readonly ILibraryRepository _libraryRepository;

    public GetLibraryByIdQueryHandler(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<LibraryDto> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
    {
        var library = await _libraryRepository.GetByIdAsync(request.Id);

        _ = library ?? throw new NotFoundException(nameof(Domain.Entities.Library), request.Id);

        return new LibraryDto { Id = library.Id, Name = library.Name, BranchLocation = library.BranchLocation };
    }
}