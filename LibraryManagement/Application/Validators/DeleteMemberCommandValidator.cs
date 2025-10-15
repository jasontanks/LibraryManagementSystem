using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class DeleteMemberCommandValidator : AbstractValidator<DeleteMemberCommand>
{
    private readonly IMemberRepository _memberRepository;

    public DeleteMemberCommandValidator(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required.")
            .MustAsync(MemberMustExist).WithMessage("Member not found.");
    }

    private async Task<bool> MemberMustExist(Guid id, CancellationToken cancellationToken)
    {
        return await _memberRepository.GetByIdAsync(id) is not null;
    }
}