using IsuTasks.Api.Domain.Entities;
using IsuTasks.Api.Domain.Results;
using IsuTasks.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IsuTasks.Api.Services.Auth;

public class AuthService(
    IsuTasksDbContext dbContext,
    ITokenGenerator tokenGenerator,
    IHasher hasher
) : IAuthService
{
    private readonly IsuTasksDbContext _dbContext = dbContext;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
    private readonly IHasher _hasher = hasher;

    public async Task<Result<AuthResult>> LoginAsync(string email, string password)
    {
        // Find user by email
        var user = await _dbContext.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.Email == email);
        if (user is null)
            return Error.NotFound($"User with email {email} was not found");

        // Check password
        if (!_hasher.Verify(password, user.Password))
            return Error.Conflict($"Wrong password");

        // Remove expired tokens
        user.RefreshTokens.RemoveAll(token => token.ExpiresAtUtc < DateTime.UtcNow); // TODO: This should be handled by a background process
        await _dbContext.SaveChangesAsync();

        // Add refresh token
        var refreshToken = _tokenGenerator.GenerateRefreshToken();
        var refreshTokenHash = _hasher.Hash(refreshToken);
        await _dbContext.RefreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Hash = refreshTokenHash,
            ExpiresAtUtc = DateTime.UtcNow.AddMonths(1),
            UserId = user.Id
        });
        await _dbContext.SaveChangesAsync();

        // Generate access token
        var accessToken = _tokenGenerator.GenerateAccessToken(new AccessTokenGenerationParameters(
            UserId: user.Id,
            Email: user.Email
        ));

        return new AuthResult(accessToken, refreshToken);
    }

    public async Task<Result> LogoutAsync(Guid userId, string refreshTokenStr)
    {
        // Find user by id
        var user = await _dbContext.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.Id == userId);
        if (user is null)
            return Error.NotFound($"User with ID {userId} was not found");

        // Remove refresh token
        var refreshToken = user.RefreshTokens.FirstOrDefault(token => _hasher.Verify(refreshTokenStr, token.Hash));
        if (refreshToken is null)
            return Error.Unauthorized($"Token {refreshTokenStr} does not belong to user with ID {userId}");
        user.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<AuthResult>> RefreshAsync(Guid userId, string refreshTokenStr)
    {
        // Find user by id
        var user = await _dbContext.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.Id == userId);
        if (user is null)
            return Error.NotFound($"User with ID {userId} was not found");

        // Remove refresh token
        var refreshToken = user.RefreshTokens.FirstOrDefault(token => _hasher.Verify(refreshTokenStr, token.Hash));
        if (refreshToken is null)
            return Error.Unauthorized($"Token {refreshTokenStr} does not belong to user with ID {userId}");
        user.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();

        // Add refresh token
        var newRefreshTokenStr = _tokenGenerator.GenerateRefreshToken();
        var newRefreshTokenHash = _hasher.Hash(newRefreshTokenStr);
        await _dbContext.RefreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Hash = newRefreshTokenHash,
            ExpiresAtUtc = DateTime.UtcNow.AddMonths(1),
            UserId = user.Id
        });
        await _dbContext.SaveChangesAsync();

        var accessToken = _tokenGenerator.GenerateAccessToken(new AccessTokenGenerationParameters(
            UserId: user.Id,
            Email: user.Email
        ));

        return new AuthResult(accessToken, newRefreshTokenStr);
    }

    public async Task<Result<AuthResult>> RegisterAsync(string email, string password)
    {
        // Verify email is not already registered
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        if (existingUser is not null)
            return Error.Conflict($"User with email {email} is already registered");

        // Create user
        var passwordHash = _hasher.Hash(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = passwordHash,
            RefreshTokens = []
        };
        await _dbContext.Users.AddAsync(user);

        // Add refresh token
        var refreshToken = _tokenGenerator.GenerateRefreshToken();
        var refreshTokenHash = _hasher.Hash(refreshToken);
        user.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Hash = refreshTokenHash,
            ExpiresAtUtc = DateTime.UtcNow.AddMonths(1)
        });
        await _dbContext.SaveChangesAsync();

        // Generate access token
        var accessToken = _tokenGenerator.GenerateAccessToken(new AccessTokenGenerationParameters(
            UserId: user.Id,
            Email: user.Email
        ));

        return new AuthResult(accessToken, refreshToken);
    }
}
