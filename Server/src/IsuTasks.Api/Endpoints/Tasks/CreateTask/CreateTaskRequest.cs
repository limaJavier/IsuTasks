namespace IsuTasks.Api.Endpoints.Tasks.CreateTask;

public record CreateTaskRequest(
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted
);
