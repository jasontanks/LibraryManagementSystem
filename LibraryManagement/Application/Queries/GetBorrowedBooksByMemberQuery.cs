using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetBorrowedBooksByMemberQuery : IRequest<IEnumerable<BorrowRecordDto>>
{
    public Guid MemberId { get; set; }

    public GetBorrowedBooksByMemberQuery(Guid memberId) => MemberId = memberId;
}