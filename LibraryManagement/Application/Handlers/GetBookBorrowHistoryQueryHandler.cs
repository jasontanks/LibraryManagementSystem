using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetBookBorrowHistoryQueryHandler : IRequestHandler<GetBookBorrowHistoryQuery, IEnumerable<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly ILogger<GetBookBorrowHistoryQueryHandler> _logger;

    public GetBookBorrowHistoryQueryHandler(IBorrowRecordRepository borrowRecordRepository, ILogger<GetBookBorrowHistoryQueryHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BorrowRecordDto>> Handle(GetBookBorrowHistoryQuery request, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving borrow history for book ID {BookId}.", request.BookId);
            throw;
        }
    }
}