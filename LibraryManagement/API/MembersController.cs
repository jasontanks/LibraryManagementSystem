using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{memberId}")]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets a specific member by their unique ID.")]
    public async Task<IActionResult> GetMemberById(Guid memberId)
    {
        var query = new GetMemberByIdQuery(memberId);
        var member = await _mediator.Send(query);
        return Ok(member);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<MemberDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Gets a paginated list of all members.")]
    public async Task<IActionResult> GetAllMembers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllMembersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var members = await _mediator.Send(query);
        return Ok(members);
    }

    [HttpGet("{memberId}/borrowed-books")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets a list of books currently borrowed by a specific member.")]
    public async Task<IActionResult> GetBorrowedBooks(Guid memberId)
    {
        var query = new GetBorrowedBooksByMemberQuery(memberId);
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{memberId}/borrow-history")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets the full borrowing history for a specific member.")]
    public async Task<IActionResult> GetBorrowHistory(Guid memberId)
    {
        var query = new GetMemberBorrowHistoryQuery(memberId);
        var history = await _mediator.Send(query);
        return Ok(history);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Registers a new member in a library.")]
    public async Task<IActionResult> RegisterMember([FromBody] RegisterMemberCommand command)
    {
        var memberDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMemberById), new { id = memberDto.Id }, memberDto);
    }

    [HttpPut("{memberId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Updates the details of an existing member.")]
    public async Task<IActionResult> UpdateMember(Guid memberId, [FromBody] UpdateMemberCommand command)
    {
        if (memberId != command.Id)
        {
            return BadRequest("ID in URL must match ID in body.");
        }

        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{memberId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Deletes a specific member.")]
    public async Task<IActionResult> DeleteMember(Guid memberId)
    {
        var command = new DeleteMemberCommand(memberId);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}