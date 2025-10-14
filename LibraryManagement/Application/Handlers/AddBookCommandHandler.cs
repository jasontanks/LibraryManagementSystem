using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public AddBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            LibraryId = request.LibraryId
        };

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            LibraryId = book.LibraryId
        };
    }
}