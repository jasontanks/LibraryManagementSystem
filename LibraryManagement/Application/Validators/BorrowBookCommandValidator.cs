using FluentValidation;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Validators;

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public BorrowBookCommandValidator(IBookRepository bookRepository, IMemberRepository memberRepository, IBorrowRecordRepository borrowRecordRepository)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _borrowRecordRepository = borrowRecordRepository;

        RuleFor(x => x.BookId)
            .NotEmpty()
            .MustAsync(BookMustExist).WithMessage("Book not found.");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .MustAsync(MemberMustExist).WithMessage("Member not found.");
    }

    private async Task<bool> BookMustExist(Guid bookId, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetByIdAsync(bookId) is not null;
    }

    private async Task<bool> MemberMustExist(Guid memberId, CancellationToken cancellationToken)
    {
        return await _memberRepository.GetByIdAsync(memberId) is not null;
    }
}