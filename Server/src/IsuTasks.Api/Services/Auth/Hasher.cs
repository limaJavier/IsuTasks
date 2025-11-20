using BCryptHasher = BCrypt.Net.BCrypt;
namespace IsuTasks.Api.Services.Auth;

public class Hasher : IHasher
{
    public string Hash(string text)
    {
        return BCryptHasher.HashPassword(text);
    }

    public bool Verify(string text, string hash)
    {
        return BCryptHasher.Verify(text, hash);
    }
}
