using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetMembersByLibraryQueryHandler : IRequestHandler<GetMembersByLibraryQuery, IEnumerable<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;

    public GetMembersByLibraryQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<MemberDto>> Handle(GetMembersByLibraryQuery request, CancellationToken cancellationToken)
    {
        var members = await _memberRepository.GetByLibraryIdAsync(request.LibraryId);

        return members.Select(m => new MemberDto
        {
            Id = m.Id,
            FullName = m.FullName,
            LibraryId = m.LibraryId
        });
    }
}