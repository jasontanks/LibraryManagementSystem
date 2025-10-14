using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetBookBorrowHistoryQueryHandler : IRequestHandler<GetBookBorrowHistoryQuery, IEnumerable<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public GetBookBorrowHistoryQueryHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<IEnumerable<BorrowRecordDto>> Handle(GetBookBorrowHistoryQuery request, CancellationToken cancellationToken)
    {
        var records = await _borrowRecordRepository.GetBorrowHistoryByBookAsync(request.BookId);
        return records.Select(br => new BorrowRecordDto
        {
            Id = br.Id,
            BookId = br.BookId,
            BookTitle = br.Book?.Title ?? string.Empty,
            MemberId = br.MemberId,
            MemberName = br.Member?.FullName ?? string.Empty,
            BorrowedAt = br.BorrowedAt,
            ReturnedAt = br.ReturnedAt
        });
    }
}