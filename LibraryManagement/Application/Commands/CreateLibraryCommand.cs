using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Commands;

public class CreateLibraryCommand : IRequest<LibraryDto>
{
    public string Name { get; set; } = string.Empty;
    public string BranchLocation { get; set; } = string.Empty;
}