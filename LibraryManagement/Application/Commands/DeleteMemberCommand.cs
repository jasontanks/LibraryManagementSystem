using MediatR;

namespace LibraryManagement.Application.Commands;

public class DeleteMemberCommand : IRequest<bool>
{
    public Guid MemberId { get; set; }
    public Guid Id { get; set; }

    public DeleteMemberCommand(Guid memberId)
    public DeleteMemberCommand(Guid id)
    {
        MemberId = memberId;
        Id = id;
    }
}