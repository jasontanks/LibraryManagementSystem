using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetMemberByIdQuery : IRequest<MemberDto>
{
    public Guid Id { get; set; }

    public GetMemberByIdQuery(Guid id) => Id = id;
}