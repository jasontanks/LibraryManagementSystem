using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetMemberBorrowHistoryQueryHandler : IRequestHandler<GetMemberBorrowHistoryQuery, IEnumerable<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly ILogger<GetMemberBorrowHistoryQueryHandler> _logger;

    public GetMemberBorrowHistoryQueryHandler(IBorrowRecordRepository borrowRecordRepository, ILogger<GetMemberBorrowHistoryQueryHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BorrowRecordDto>> Handle(GetMemberBorrowHistoryQuery request, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving borrow history for member ID {MemberId}.", request.MemberId);
            throw;
        }
    }
}