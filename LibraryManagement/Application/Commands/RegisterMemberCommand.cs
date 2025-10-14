using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Commands;

public class RegisterMemberCommand : IRequest<MemberDto>
{
    public string FullName { get; set; } = string.Empty;
    public Guid LibraryId { get; set; }
}