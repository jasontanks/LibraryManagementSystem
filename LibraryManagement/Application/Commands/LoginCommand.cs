using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Commands;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}