using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class UpdateLibraryCommandValidator : AbstractValidator<UpdateLibraryCommand>
{
    private readonly ILibraryRepository _libraryRepository;

    public UpdateLibraryCommandValidator(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(LibraryMustExist).WithMessage("Library not found.");

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.BranchLocation).NotEmpty();
    }

    private async Task<bool> LibraryMustExist(Guid id, CancellationToken cancellationToken)
    {
        return await _libraryRepository.GetByIdAsync(id) is not null;
    }
}