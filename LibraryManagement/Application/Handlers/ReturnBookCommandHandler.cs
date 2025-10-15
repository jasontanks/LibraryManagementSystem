using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Handlers;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, bool>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly ILogger<ReturnBookCommandHandler> _logger;

    public ReturnBookCommandHandler(IBorrowRecordRepository borrowRecordRepository, ILogger<ReturnBookCommandHandler> logger)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var borrowRecord = await _borrowRecordRepository.GetByIdAsync(request.Id);

            // The validator ensures the record exists and is active, but a defensive check is good practice.
            if (borrowRecord is null || borrowRecord.ReturnedAt is not null)
            {
                return false;
            }

            borrowRecord.ReturnedAt = DateTime.UtcNow;
            var result = await _borrowRecordRepository.SaveChangesAsync();

            if (result)
            {
                _logger.LogInformation("Book return processed successfully for BorrowRecordId {BorrowRecordId}.", request.Id);
            }
            return result;
        }
        catch (DatabaseOperationException ex)
        {
            _logger.LogError(ex, "A database operation failed while processing a book return for BorrowRecordId {BorrowRecordId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while processing a book return for BorrowRecordId {BorrowRecordId}.", request.Id);
            throw;
        }
    }
}