using MediatR;

namespace LibraryManagement.Application.Commands;

public class DeleteLibraryCommand : IRequest<bool>
{
    public Guid Id { get; }

    public DeleteLibraryCommand(Guid id) => Id = id;
}