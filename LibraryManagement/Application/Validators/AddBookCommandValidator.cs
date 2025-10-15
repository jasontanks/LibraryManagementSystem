using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
{
    private readonly ILibraryRepository _libraryRepository;

    public AddBookCommandValidator(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.LibraryId)
            .NotEmpty().WithMessage("LibraryId is required.")
            .MustAsync(LibraryMustExist).WithMessage("The specified library does not exist.");
    }

    private async Task<bool> LibraryMustExist(Guid libraryId, CancellationToken cancellationToken)
    {
        return await _libraryRepository.GetByIdAsync(libraryId) is not null;
    }
}