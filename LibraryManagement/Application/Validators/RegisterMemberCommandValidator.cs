using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class RegisterMemberCommandValidator : AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberCommandValidator(IMemberRepository memberRepository, ILibraryRepository libraryRepository)
    {
        RuleFor(x => x.LibraryId)
            .NotEmpty().WithMessage("LibraryId is required.");
        RuleFor(x => x.FullName)
          .NotEmpty().WithMessage("Name is required.");
    }
}