using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetMembersByLibraryQueryHandler : IRequestHandler<GetMembersByLibraryQuery, IEnumerable<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<GetMembersByLibraryQueryHandler> _logger;

    public GetMembersByLibraryQueryHandler(IMemberRepository memberRepository, ILogger<GetMembersByLibraryQueryHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MemberDto>> Handle(GetMembersByLibraryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var members = await _memberRepository.GetByLibraryIdAsync(request.LibraryId);

            return members.Select(m => new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                LibraryId = m.LibraryId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving members for library ID {LibraryId}.", request.LibraryId);
            throw;
        }
    }
}