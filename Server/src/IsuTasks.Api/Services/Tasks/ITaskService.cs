using IsuTasks.Api.Domain.Entities;
using IsuTasks.Api.Domain.Results;

namespace IsuTasks.Api.Services.Tasks;

public interface ITaskService
{
    // Queries
    Task<Result<List<IsuTask>>> GetTasksAsync();
    Task<Result<IsuTask>> GetTaskByIdAsync(Guid id);

    // Commands
    Task<Result> CreateTaskAsync(IsuTask task);
    Task<Result> UpdateTaskAsync(IsuTask task);
    Task<Result> DeleteTaskAsync(Guid id);
}
