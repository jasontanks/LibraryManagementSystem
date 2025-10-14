using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class RegisterMemberCommandValidator : AbstractValidator<RegisterMemberCommand>
{
    private readonly ILibraryRepository _libraryRepository;

    public RegisterMemberCommandValidator(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required.")
            .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

        RuleFor(x => x.LibraryId)
            .NotEmpty().WithMessage("LibraryId is required.")
            .MustAsync(LibraryMustExist).WithMessage("The specified library does not exist.");
    }

    private async Task<bool> LibraryMustExist(Guid libraryId, CancellationToken cancellationToken)
    {
        return await _libraryRepository.ExistsAsync(libraryId);
    }
}