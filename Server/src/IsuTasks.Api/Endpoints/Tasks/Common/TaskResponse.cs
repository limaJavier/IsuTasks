namespace IsuTasks.Api.Endpoints.Tasks.Common;

public record TaskResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted
);
