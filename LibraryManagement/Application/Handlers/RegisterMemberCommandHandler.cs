using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class RegisterMemberCommandHandler : IRequestHandler<RegisterMemberCommand, MemberDto>
{
    private readonly IMemberRepository _memberRepository;

    public RegisterMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<MemberDto> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            LibraryId = request.LibraryId
        };

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        return new MemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            LibraryId = member.LibraryId
        };
    }
}