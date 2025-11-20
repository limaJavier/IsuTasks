using IsuTasks.Api.Domain.Results;

namespace IsuTasks.Api.Services.Auth;

public interface IAuthService
{
    Task<Result<AuthResult>> RegisterAsync(string email, string password);
    Task<Result<AuthResult>> LoginAsync(string email, string password);
    Task<Result<AuthResult>> RefreshAsync(Guid userId, string refreshToken);
    Task<Result> LogoutAsync(Guid userId, string refreshToken);
}

public record AuthResult(
    string AccessToken,
    string RefreshToken
);
