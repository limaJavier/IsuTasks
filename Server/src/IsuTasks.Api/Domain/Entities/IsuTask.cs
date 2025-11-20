namespace IsuTasks.Api.Domain.Entities;

public class IsuTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    // Relational properties
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
