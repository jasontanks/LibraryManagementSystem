using FluentValidation;
using LibraryManagement.Application.Commands;

namespace LibraryManagement.Application.Validators;

public class CreateLibraryCommandValidator : AbstractValidator<CreateLibraryCommand>
{
    public CreateLibraryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Library name is required.");

        RuleFor(x => x.BranchLocation)
            .NotEmpty().WithMessage("Branch location is required.");
    }
}