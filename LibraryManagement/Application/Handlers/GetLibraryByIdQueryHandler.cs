using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetLibraryByIdQueryHandler : IRequestHandler<GetLibraryByIdQuery, LibraryDto>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<GetLibraryByIdQueryHandler> _logger;

    public GetLibraryByIdQueryHandler(ILibraryRepository libraryRepository, ILogger<GetLibraryByIdQueryHandler> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<LibraryDto> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var library = await _libraryRepository.GetByIdAsync(request.Id);

            _ = library ?? throw new NotFoundException(nameof(Domain.Entities.Library), request.Id);

            _logger.LogInformation("Successfully retrieved library with ID {LibraryId}.", request.Id);

            return new LibraryDto { Id = library.Id, Name = library.Name, BranchLocation = library.BranchLocation };
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving library with ID {LibraryId}.", request.Id);
            throw;
        }
    }
}