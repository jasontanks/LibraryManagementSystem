using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<GetMemberByIdQueryHandler> _logger;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository, ILogger<GetMemberByIdQueryHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var member = await _memberRepository.GetByIdAsync(request.Id);

            _ = member ?? throw new NotFoundException(nameof(Domain.Entities.Member), request.Id);

            _logger.LogInformation("Successfully retrieved member with ID {MemberId}.", request.Id);

            return new MemberDto { Id = member.Id, FullName = member.FullName, LibraryId = member.LibraryId };
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving member with ID {MemberId}.", request.Id);
            throw;
        }
    }
}