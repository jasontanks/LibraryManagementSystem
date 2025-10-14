using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Application.Handlers;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, BorrowRecordDto>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public BorrowBookCommandHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<BorrowRecordDto> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        // Business Rule: Check if the book is already borrowed.
        if (await _borrowRecordRepository.GetActiveBorrowRecordForBookAsync(request.BookId) is not null)
            throw new BookAlreadyBorrowedException(request.BookId);

        var borrowRecord = new BorrowRecord
        {
            Id = Guid.NewGuid(),
            BookId = request.BookId,
            MemberId = request.MemberId,
            BorrowedAt = DateTime.UtcNow
        };

        await _borrowRecordRepository.AddAsync(borrowRecord);
        await _borrowRecordRepository.SaveChangesAsync();

        // We need to fetch the book title to return a complete DTO
        var createdRecord = await _borrowRecordRepository.GetByIdAsync(borrowRecord.Id);

        return new BorrowRecordDto
        {
            Id = createdRecord!.Id,
            BookId = createdRecord.BookId,
            BookTitle = createdRecord.Book?.Title ?? string.Empty, // Assumes GetByIdAsync includes the Book
            MemberId = createdRecord.MemberId,
            MemberName = createdRecord.Member?.FullName ?? string.Empty, // Assumes GetByIdAsync includes the Member
            BorrowedAt = createdRecord.BorrowedAt,
            ReturnedAt = createdRecord.ReturnedAt
        };
    }
}