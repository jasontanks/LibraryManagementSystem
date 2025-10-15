using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, LibraryDto>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<CreateLibraryCommandHandler> _logger;

    public CreateLibraryCommandHandler(ILibraryRepository libraryRepository, ILogger<CreateLibraryCommandHandler> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<LibraryDto> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var library = new Library
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                BranchLocation = request.BranchLocation
            };

            await _libraryRepository.AddAsync(library);
            await _libraryRepository.SaveChangesAsync();

            _logger.LogInformation("Library with ID {LibraryId} was created successfully.", library.Id);

            return new LibraryDto { Id = library.Id, Name = library.Name, BranchLocation = library.BranchLocation };
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while creating a library with name '{LibraryName}'.", request.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a new library with name '{LibraryName}'.", request.Name);
            throw;
        }
    }
}