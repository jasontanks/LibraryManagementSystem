using MediatR;

namespace LibraryManagement.Application.Commands;

public class DeleteMemberCommand : IRequest<bool>
{
    public Guid MemberId { get; set; }

    public DeleteMemberCommand(Guid memberId)
    {
        MemberId = memberId;
    }
}