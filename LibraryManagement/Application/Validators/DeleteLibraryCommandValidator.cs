using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class DeleteLibraryCommandValidator : AbstractValidator<DeleteLibraryCommand>
{
    private readonly ILibraryRepository _libraryRepository;

    public DeleteLibraryCommandValidator(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Library ID is required.")
            .MustAsync(LibraryMustExist).WithMessage("Library not found.");
    }

    private async Task<bool> LibraryMustExist(Guid id, CancellationToken cancellationToken)
    {
        return await _libraryRepository.GetByIdAsync(id) is not null;
    }
}