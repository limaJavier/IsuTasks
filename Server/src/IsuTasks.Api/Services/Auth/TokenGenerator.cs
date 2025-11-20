using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IsuTasks.Api.Services.Auth;

public class TokenGenerator : ITokenGenerator
{
    public string GenerateAccessToken(AccessTokenGenerationParameters generationParameters)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("An at least 256 bits signing key")); // Build key
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Build credentials from key

        // Define claims
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token's id
            new Claim(JwtRegisteredClaimNames.Sub, generationParameters.UserId.ToString()), // User's id
            new Claim(JwtRegisteredClaimNames.Email, generationParameters.Email.ToString()) // User's email
        };

        // TODO: Token info shouldn't be hardcoded
        // Build token
        var token = new JwtSecurityToken(
            issuer: "isu.task.auth",
            audience: "isu.task",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(int size)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(size));
    }
}
