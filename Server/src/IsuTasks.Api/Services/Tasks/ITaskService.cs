using IsuTasks.Api.Domain.Entities;
using IsuTasks.Api.Domain.Results;

namespace IsuTasks.Api.Services.Tasks;

public interface ITaskService
{
    // Queries
    Task<Result<List<IsuTask>>> GetTasksAsync(Guid userId);
    Task<Result<IsuTask>> GetTaskByIdAsync(Guid taskId, Guid userId);

    // Commands
    Task<Result> CreateTaskAsync(IsuTask task);
    Task<Result> UpdateTaskAsync(IsuTask task, Guid userId);
    Task<Result> DeleteTaskAsync(Guid taskId, Guid userId);
}
