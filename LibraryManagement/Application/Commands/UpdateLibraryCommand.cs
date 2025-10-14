using MediatR;

namespace LibraryManagement.Application.Commands;

public class UpdateLibraryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BranchLocation { get; set; } = string.Empty;
}