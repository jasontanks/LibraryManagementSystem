using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class UpdateLibraryCommandHandler : IRequestHandler<UpdateLibraryCommand, bool>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<UpdateLibraryCommandHandler> _logger;

    public UpdateLibraryCommandHandler(ILibraryRepository libraryRepository, ILogger<UpdateLibraryCommandHandler> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var library = await _libraryRepository.GetByIdAsync(request.Id);
            if (library is null)
            {
                return false;
            }

            library.Name = request.Name;
            library.BranchLocation = request.BranchLocation;
            var result = await _libraryRepository.SaveChangesAsync();

            if (result)
            {
                _logger.LogInformation("Library with ID {LibraryId} was updated successfully.", request.Id);
            }
            return result;
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while updating library with ID {LibraryId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating library with ID {LibraryId}.", request.Id);
            throw;
        }
    }
}