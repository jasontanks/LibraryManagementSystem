using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, bool>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<UpdateMemberCommandHandler> _logger;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository, ILogger<UpdateMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var member = await _memberRepository.GetByIdAsync(request.Id);

            if (member is null)
            {
                return false;
            }

            member.FullName = request.FullName;
            var result = await _memberRepository.SaveChangesAsync();
            if (result)
            {
                _logger.LogInformation("Member with ID {MemberId} was updated successfully.", request.Id);
            }
            return result;
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while updating member with ID {MemberId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating member with ID {MemberId}.", request.Id);
            throw;
        }
    }
}