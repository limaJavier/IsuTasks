namespace IsuTasks.Api.Services.Auth;

public interface ITokenGenerator
{
    string GenerateAccessToken(AccessTokenGenerationParameters userId);
    string GenerateRefreshToken(int size = 64);
}

public record AccessTokenGenerationParameters(
    Guid UserId,
    string Email
);
