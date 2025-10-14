using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetBorrowRecordByIdQuery : IRequest<BorrowRecordDto>
{
    public Guid Id { get; init; }

    public GetBorrowRecordByIdQuery(Guid id) => Id = id;
}