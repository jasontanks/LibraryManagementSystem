using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetAllLibrariesQuery : IRequest<IEnumerable<LibraryDto>>
{
}