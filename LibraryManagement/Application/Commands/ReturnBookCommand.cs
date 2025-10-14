using MediatR;

namespace LibraryManagement.Application.Commands;

public class ReturnBookCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}