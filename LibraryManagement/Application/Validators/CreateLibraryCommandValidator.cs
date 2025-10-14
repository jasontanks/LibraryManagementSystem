using FluentValidation;
using LibraryManagement.Application.Commands;

namespace LibraryManagement.Application.Validators;

public class CreateLibraryCommandValidator : AbstractValidator<CreateLibraryCommand>
{
    public CreateLibraryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BranchLocation).NotEmpty().MaximumLength(200);
    }
}