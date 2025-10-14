using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, LibraryDto>
{
    private readonly ILibraryRepository _libraryRepository;

    public CreateLibraryCommandHandler(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<LibraryDto> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
    {
        var library = new Library
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            BranchLocation = request.BranchLocation
        };

        await _libraryRepository.AddAsync(library);
        await _libraryRepository.SaveChangesAsync();

        return new LibraryDto { Id = library.Id, Name = library.Name, BranchLocation = library.BranchLocation };
    }
}