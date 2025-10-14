using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class DeleteLibraryCommandHandler : IRequestHandler<DeleteLibraryCommand, bool>
{
    private readonly ILibraryRepository _libraryRepository;

    public DeleteLibraryCommandHandler(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<bool> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        var library = await _libraryRepository.GetByIdAsync(request.Id);
        if (library is null)
        {
            return false;
        }

        _libraryRepository.Delete(library);
        return await _libraryRepository.SaveChangesAsync();
    }
}