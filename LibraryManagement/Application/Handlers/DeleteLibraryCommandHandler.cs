using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class DeleteLibraryCommandHandler : IRequestHandler<DeleteLibraryCommand, bool>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<DeleteLibraryCommandHandler> _logger;

    public DeleteLibraryCommandHandler(ILibraryRepository libraryRepository, ILogger<DeleteLibraryCommandHandler> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var library = await _libraryRepository.GetByIdAsync(request.Id);
            if (library is null)
            {
                return false;
            }

            _libraryRepository.Delete(library);
            var result = await _libraryRepository.SaveChangesAsync();

            if (result)
            {
                _logger.LogInformation("Library with ID {LibraryId} was deleted successfully.", request.Id);
            }
            return result;
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while deleting library with ID {LibraryId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting library with ID {LibraryId}.", request.Id);
            throw;
        }
    }
}