using MediatR;

namespace LibraryManagement.Application.Commands;

public class UpdateMemberCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}