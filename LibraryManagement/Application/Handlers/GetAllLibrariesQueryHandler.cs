using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetAllLibrariesQueryHandler : IRequestHandler<GetAllLibrariesQuery, IEnumerable<LibraryDto>>
{
    private readonly ILibraryRepository _libraryRepository;

    public GetAllLibrariesQueryHandler(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<IEnumerable<LibraryDto>> Handle(GetAllLibrariesQuery request, CancellationToken cancellationToken)
    {
        var libraries = await _libraryRepository.GetAllAsync();
        return libraries.Select(l => new LibraryDto
        {
            Id = l.Id,
            Name = l.Name,
            BranchLocation = l.BranchLocation
        });
    }
}