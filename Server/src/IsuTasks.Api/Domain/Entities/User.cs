namespace IsuTasks.Api.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    // Relational properties
    public List<IsuTask> Tasks { get; set; } = null!;
    public List<RefreshToken> RefreshTokens { get; set; } = null!;
}
