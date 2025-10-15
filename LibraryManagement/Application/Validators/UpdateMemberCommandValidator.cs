using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;

    public UpdateMemberCommandValidator(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(MemberMustExist).WithMessage("Member not found.");
    }

    private async Task<bool> MemberMustExist(Guid id, CancellationToken cancellationToken)
    {
        return await _memberRepository.GetByIdAsync(id) is not null;
    }
}