using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, PaginatedList<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<GetAllMembersQueryHandler> _logger;

    public GetAllMembersQueryHandler(IMemberRepository memberRepository, ILogger<GetAllMembersQueryHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<PaginatedList<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving all members.");
            throw;
        }
    }
}