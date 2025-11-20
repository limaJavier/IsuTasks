using IsuTasks.Api.Domain.Entities;
using IsuTasks.Api.Domain.Results;
using IsuTasks.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IsuTasks.Api.Services.Tasks;

public class TaskService(IsuTasksDbContext dbContext) : ITaskService
{
    private readonly IsuTasksDbContext _dbContext = dbContext;

    public async Task<Result> CreateTaskAsync(IsuTask task)
    {
        task.Id = Guid.NewGuid();
        try
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Error.Unexpected(e.Message);
        }
    }

    public async Task<Result> DeleteTaskAsync(Guid taskId, Guid userId)
    {
        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(task =>
                task.Id == taskId &&
                task.UserId == userId
            );
        if (task is null)
            return Error.NotFound($"Task with ID {taskId} was not found");

        try
        {
            _dbContext.Remove(task);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Error.Unexpected(e.Message);
        }
    }

    public async Task<Result<IsuTask>> GetTaskByIdAsync(Guid taskId, Guid userId)
    {
        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(task =>
                task.Id == taskId &&
                task.UserId == userId
            );

        if (task is null)
            return Error.NotFound($"Task with ID {taskId} was not found");
        else
            return task;
    }

    public async Task<Result<List<IsuTask>>> GetTasksAsync(Guid userId)
    {
        return await _dbContext.Tasks
            .Where(task => task.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// This method assumes that the given task is being tracked by EF Core, therefore we only need to "SaveChanges"
    /// </summary>
    public async Task<Result> UpdateTaskAsync(IsuTask task, Guid userId)
    {
        if (task.UserId != userId)
            return Error.NotFound($"Task with ID {task.Id} was not found");

        try
        {
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Error.Unexpected(e.Message);
        }
    }
}
