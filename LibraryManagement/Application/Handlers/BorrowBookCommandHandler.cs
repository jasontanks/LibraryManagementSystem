using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, BorrowRecordDto>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly ILogger<BorrowBookCommandHandler> _logger;

    public BorrowBookCommandHandler(IBorrowRecordRepository borrowRecordRepository, ILogger<BorrowBookCommandHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _logger = logger;
    }

    public async Task<BorrowRecordDto> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        try
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

            _logger.LogInformation("Book with ID {BookId} was borrowed by Member {MemberId}. Record ID: {BorrowRecordId}", request.BookId, request.MemberId, borrowRecord.Id);

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
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while creating a borrow record for BookId {BookId} and MemberId {MemberId}.", request.BookId, request.MemberId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a borrow record for BookId {BookId} and MemberId {MemberId}.", request.BookId, request.MemberId);
            throw;
        }
    }
}