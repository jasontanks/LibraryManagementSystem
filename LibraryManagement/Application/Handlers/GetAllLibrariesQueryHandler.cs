using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetAllLibrariesQueryHandler : IRequestHandler<GetAllLibrariesQuery, IEnumerable<LibraryDto>>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<GetAllLibrariesQueryHandler> _logger;

    public GetAllLibrariesQueryHandler(ILibraryRepository libraryRepository, ILogger<GetAllLibrariesQueryHandler> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<LibraryDto>> Handle(GetAllLibrariesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var libraries = await _libraryRepository.GetAllAsync();
            var libraryDtos = libraries.Select(l => new LibraryDto
            {
                Id = l.Id,
                Name = l.Name,
                BranchLocation = l.BranchLocation
            }).ToList();

            _logger.LogInformation("Successfully retrieved {LibraryCount} libraries.", libraryDtos.Count);

            return libraryDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving all libraries.");
            throw;
        }
    }
}