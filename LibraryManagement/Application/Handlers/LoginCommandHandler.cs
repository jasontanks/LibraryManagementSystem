using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        var roles = new List<string> { "Admin" };

        var token = _jwtTokenGenerator.GenerateToken(userId, request.Username, roles);

        return Task.FromResult(new LoginResponseDto { Token = token });
    }
}