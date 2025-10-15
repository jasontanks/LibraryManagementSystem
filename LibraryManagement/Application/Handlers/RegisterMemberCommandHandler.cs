using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class RegisterMemberCommandHandler : IRequestHandler<RegisterMemberCommand, MemberDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<RegisterMemberCommandHandler> _logger;

    public RegisterMemberCommandHandler(IMemberRepository memberRepository, ILogger<RegisterMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<MemberDto> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var member = new Member
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                LibraryId = request.LibraryId
            };

            await _memberRepository.AddAsync(member);
            await _memberRepository.SaveChangesAsync();

            _logger.LogInformation("Member with ID {MemberId} was registered successfully.", member.Id);

            return new MemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                LibraryId = member.LibraryId
            };
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while registering a member with name '{MemberName}'.", request.FullName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while registering a new member with name '{MemberName}'.", request.FullName);
            throw;
        }
    }
}