using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetMemberBorrowHistoryQueryHandler : IRequestHandler<GetMemberBorrowHistoryQuery, IEnumerable<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public GetMemberBorrowHistoryQueryHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<IEnumerable<BorrowRecordDto>> Handle(GetMemberBorrowHistoryQuery request, CancellationToken cancellationToken)
    {
        var records = await _borrowRecordRepository.GetBorrowHistoryByMemberAsync(request.MemberId);
        return records.Select(br => new BorrowRecordDto
        {
            Id = br.Id,
            BookId = br.BookId,
            BookTitle = br.Book?.Title ?? string.Empty,
            MemberId = br.MemberId,
            MemberName = br.Member?.FullName ?? string.Empty, // This will be null if Member is not included in the query
            BorrowedAt = br.BorrowedAt,
            ReturnedAt = br.ReturnedAt
        });
    }
}