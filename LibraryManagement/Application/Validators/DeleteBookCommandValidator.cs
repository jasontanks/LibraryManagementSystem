using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandValidator(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;

        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.")
            .MustAsync(BookMustExist).WithMessage("Book not found.");
    }

    private async Task<bool> BookMustExist(Guid id, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetByIdAsync(id) is not null;
    }
}