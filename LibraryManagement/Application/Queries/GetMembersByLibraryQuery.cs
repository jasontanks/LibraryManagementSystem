using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetMembersByLibraryQuery : IRequest<IEnumerable<MemberDto>>
{
    public Guid LibraryId { get; set; }

    public GetMembersByLibraryQuery(Guid libraryId)
    {
        LibraryId = libraryId;
    }
}