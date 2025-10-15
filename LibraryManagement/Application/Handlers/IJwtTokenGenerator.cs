namespace LibraryManagement.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username, IEnumerable<string> roles);
}