using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class GetBorrowRecordByIdQueryHandler : IRequestHandler<GetBorrowRecordByIdQuery, BorrowRecordDto>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly ILogger<GetBorrowRecordByIdQueryHandler> _logger;

    public GetBorrowRecordByIdQueryHandler(IBorrowRecordRepository borrowRecordRepository, ILogger<GetBorrowRecordByIdQueryHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _logger = logger;
    }

    public async Task<BorrowRecordDto> Handle(GetBorrowRecordByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var borrowRecord = await _borrowRecordRepository.GetByIdAsync(request.Id);

            _ = borrowRecord ?? throw new NotFoundException(nameof(Domain.Entities.BorrowRecord), request.Id);

            return new BorrowRecordDto
            {
                Id = borrowRecord.Id,
                BookId = borrowRecord.BookId,
                BookTitle = borrowRecord.Book?.Title ?? string.Empty,
                MemberId = borrowRecord.MemberId,
                MemberName = borrowRecord.Member?.FullName ?? string.Empty,
                BorrowedAt = borrowRecord.BorrowedAt,
                ReturnedAt = borrowRecord.ReturnedAt
            };
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving borrow record with ID {BorrowRecordId}.", request.Id);
            throw;
        }
    }
}