using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("v1/borrow")]
[Authorize]
[Produces("application/json")]
public class BorrowController : ControllerBase
{
    private readonly IMediator _mediator;

    public BorrowController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(BorrowRecordDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Borrows a book for a member, creating a new borrow record.")]
    public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand command)
    {
        var borrowRecordDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBorrowRecordById), new { id = borrowRecordDto.Id }, borrowRecordDto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BorrowRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets a specific borrow record by its unique ID.")]
    public async Task<IActionResult> GetBorrowRecordById(Guid id)
    {
        var query = new GetBorrowRecordByIdQuery(id);
        var record = await _mediator.Send(query);
        return record is not null ? Ok(record) : NotFound();
    }

    [HttpPatch("{id}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Marks a borrowed book as returned.")]
    public async Task<IActionResult> ReturnBook(Guid id)
    {
        var command = new ReturnBookCommand { Id = id };
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
}