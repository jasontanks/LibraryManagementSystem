using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetBorrowedBooksByMemberQueryHandler : IRequestHandler<GetBorrowedBooksByMemberQuery, IEnumerable<BorrowRecordDto>>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly ILogger<GetBorrowedBooksByMemberQueryHandler> _logger;

    public GetBorrowedBooksByMemberQueryHandler(IBorrowRecordRepository borrowRecordRepository, ILogger<GetBorrowedBooksByMemberQueryHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BorrowRecordDto>> Handle(GetBorrowedBooksByMemberQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var borrowRecords = await _borrowRecordRepository.GetBorrowedBooksByMemberAsync(request.MemberId);
            return borrowRecords.Select(br => new BorrowRecordDto
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
            _logger.LogError(ex, "An unexpected error occurred while retrieving borrowed books for member ID {MemberId}.", request.MemberId);
            throw;
        }
    }
}