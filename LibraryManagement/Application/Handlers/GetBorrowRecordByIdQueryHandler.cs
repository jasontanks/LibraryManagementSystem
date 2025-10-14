using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class GetBorrowRecordByIdQueryHandler : IRequestHandler<GetBorrowRecordByIdQuery, BorrowRecordDto>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public GetBorrowRecordByIdQueryHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<BorrowRecordDto> Handle(GetBorrowRecordByIdQuery request, CancellationToken cancellationToken)
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
}