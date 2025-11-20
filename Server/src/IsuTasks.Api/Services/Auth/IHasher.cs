namespace IsuTasks.Api.Services.Auth;

public interface IHasher
{
    string Hash(string text);
    bool Verify(string text, string hash);
}
