using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("v1/libraries")]
[Produces("application/json")]
public class LibraryController : ControllerBase
{
    private readonly IMediator _mediator;

    public LibraryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Creates a new library.")]
    public async Task<IActionResult> CreateLibrary([FromBody] CreateLibraryCommand command)
    {
        var libraryDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetLibraryById), new { libraryId = libraryDto.Id }, libraryDto);
    }

    [HttpGet("{libraryId}")]
    [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets a specific library by its unique ID.")]

    public async Task<IActionResult> GetLibraryById(Guid libraryId)
    {
        var query = new GetLibraryByIdQuery(libraryId);
        var library = await _mediator.Send(query);
        return Ok(library);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LibraryDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Gets a list of all libraries.")]
    public async Task<IActionResult> GetAllLibraries()
    {
        var libraries = await _mediator.Send(new GetAllLibrariesQuery());
        return Ok(libraries);
    }

    [HttpPut("{libraryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Updates the details of an existing library.")]

    public async Task<IActionResult> UpdateLibrary(Guid libraryId, [FromBody] UpdateLibraryCommand command)
    {
        if (libraryId != command.Id)
        {
            return BadRequest("ID in URL must match ID in body.");
        }

        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{libraryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Deletes a specific library.")]
    public async Task<IActionResult> DeleteLibrary(Guid libraryId)
    {
        var command = new DeleteLibraryCommand(libraryId);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [HttpGet("{libraryId}/books")]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Gets a list of all books within a specific library.")]
    public async Task<IActionResult> GetBooksByLibrary(Guid libraryId)
    {
        var query = new GetBooksByLibraryQuery(libraryId);
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{libraryId}/members")]
    [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Gets a list of all members registered to a specific library.")]
    public async Task<IActionResult> GetMembersByLibrary(Guid libraryId)
    {
        var query = new GetMembersByLibraryQuery(libraryId);
        var members = await _mediator.Send(query);
        return Ok(members);
    }
}