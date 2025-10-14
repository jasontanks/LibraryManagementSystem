using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, bool>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public ReturnBookCommandHandler(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<bool> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(request.Id);

        // The validator ensures the record exists and is active, but a defensive check is good practice.
        if (borrowRecord is null || borrowRecord.ReturnedAt is not null)
        {
            return false;
        }

        borrowRecord.ReturnedAt = DateTime.UtcNow;
        return await _borrowRecordRepository.SaveChangesAsync();
    }
}