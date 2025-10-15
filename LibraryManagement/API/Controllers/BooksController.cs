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
[Route("v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{bookId}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets a specific book by its unique ID.")]
    public async Task<IActionResult> GetBookById(Guid bookId)
    {
        var query = new GetBookByIdQuery(bookId);
        var book = await _mediator.Send(query);
        return Ok(book);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<BookDto>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Gets a paginated list of all books.")]
    public async Task<IActionResult> GetAllBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllBooksQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{bookId}/borrow-history")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets the full borrowing history for a specific book.")]
    public async Task<IActionResult> GetBorrowHistory(Guid bookId)
    {
        var query = new GetBookBorrowHistoryQuery(bookId);
        var history = await _mediator.Send(query);
        return Ok(history);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Adds a new book to a library.")]
    public async Task<IActionResult> AddBook([FromBody] AddBookCommand command)
    {
        var bookDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBookById), new { bookId = bookDto.Id }, bookDto);
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Updates the details of an existing book.")]
    public async Task<IActionResult> UpdateBook(Guid bookId, [FromBody] UpdateBookCommand command)
    {
        if (bookId != command.Id)
        {
            return BadRequest("ID in URL must match ID in body.");
        }

        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Deletes a specific book.")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        var command = new DeleteBookCommand(bookId);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}