using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetMemberBorrowHistoryQuery : IRequest<IEnumerable<BorrowRecordDto>>
{
    public Guid MemberId { get; }

    public GetMemberBorrowHistoryQuery(Guid memberId) => MemberId = memberId;
}