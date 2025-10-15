using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, bool>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<DeleteMemberCommandHandler> _logger;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository, ILogger<DeleteMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var member = await _memberRepository.GetByIdAsync(request.MemberId);
            if (member is null)
            {
                return false;
            }

            _memberRepository.Delete(member);
            var result = await _memberRepository.SaveChangesAsync();

            if (result)
            {
                _logger.LogInformation("Member with ID {MemberId} was deleted successfully.", request.MemberId);
            }
            return result;
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while deleting member with ID {MemberId}.", request.MemberId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting member with ID {MemberId}.", request.MemberId);
            throw;
        }
    }
}