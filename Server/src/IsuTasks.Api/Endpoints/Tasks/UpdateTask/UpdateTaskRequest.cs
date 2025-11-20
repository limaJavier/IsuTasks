namespace IsuTasks.Api.Endpoints.Tasks.UpdateTask;

public record UpdateTaskRequest(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted
);
