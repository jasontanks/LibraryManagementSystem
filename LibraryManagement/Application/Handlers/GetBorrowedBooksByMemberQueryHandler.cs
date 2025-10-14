using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetBorrowedBooksByMemberQueryHandler : IRequestHandler<GetBorrowedBooksByMemberQuery, IEnumerable<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public GetBorrowedBooksByMemberQueryHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<IEnumerable<BorrowRecordDto>> Handle(GetBorrowedBooksByMemberQuery request, CancellationToken cancellationToken)
    {
        var borrowRecords = await _borrowRecordRepository.GetBorrowedBooksByMemberAsync(request.MemberId);
        return borrowRecords.Select(br => new BorrowRecordDto
        {
            Id = br.Id,
            BookId = br.BookId,
            BookTitle = br.Book!.Title,
            MemberId = br.MemberId,
            MemberName = br.Member?.FullName ?? string.Empty, // This will be null if Member is not included in the query
            BorrowedAt = br.BorrowedAt,
            ReturnedAt = br.ReturnedAt
        });
    }
}