using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class UpdateLibraryCommandHandler : IRequestHandler<UpdateLibraryCommand, bool>
{
    private readonly ILibraryRepository _libraryRepository;

    public UpdateLibraryCommandHandler(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<bool> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
    {
        var library = await _libraryRepository.GetByIdAsync(request.Id);
        if (library is null)
        {
            return false;
        }

        library.Name = request.Name;
        library.BranchLocation = request.BranchLocation;
        return await _libraryRepository.SaveChangesAsync();
    }
}