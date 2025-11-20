namespace IsuTasks.Api.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    // Relational properties
    public ICollection<IsuTask> Tasks { get; set; } = null!;
}
