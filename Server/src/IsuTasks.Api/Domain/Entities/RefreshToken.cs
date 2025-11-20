namespace IsuTasks.Api.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Hash { get; set; } = null!;
    public DateTime ExpiresAtUtc { get; set; }

    // Relational properties
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
