using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public ReturnBookCommandValidator(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;

        RuleFor(x => x.BorrowId)
            .NotEmpty()
            .MustAsync(MustBeAnActiveBorrowRecord).WithMessage("Borrow record not found or book has already been returned.");
    }

    private async Task<bool> MustBeAnActiveBorrowRecord(Guid borrowId, CancellationToken cancellationToken)
    {
        var record = await _borrowRecordRepository.GetByIdAsync(borrowId);
        return record is not null && record.ReturnedAt is null;
    }
}