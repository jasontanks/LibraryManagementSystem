using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILibraryRepository _libraryRepository;

    public UpdateBookCommandValidator(IBookRepository bookRepository, ILibraryRepository libraryRepository)
    {
        _bookRepository = bookRepository;
        _libraryRepository = libraryRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(BookMustExist).WithMessage("Book not found.");

        RuleFor(x => x.Title).NotEmpty();
    }

    private async Task<bool> BookMustExist(Guid id, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetByIdAsync(id) is not null;
    }
}