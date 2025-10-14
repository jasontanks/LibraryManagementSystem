using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, PaginatedList<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;

    public GetAllMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<PaginatedList<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _memberRepository.GetAllAsync(request.PageNumber, request.PageSize);
        var memberDtos = items.Select(m => new MemberDto
        {
            Id = m.Id,
            FullName = m.FullName,
            LibraryId = m.LibraryId
        }).ToList();

        return new PaginatedList<MemberDto>(memberDtos, totalCount, request.PageNumber, request.PageSize);
    }
}