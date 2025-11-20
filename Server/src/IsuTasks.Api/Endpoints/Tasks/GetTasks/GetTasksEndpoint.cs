using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Tasks.Common;
using IsuTasks.Api.Services.Tasks;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api.Endpoints.Tasks.GetTasks;

public class GetTasksEndpoint(
    ITaskService taskService,
    IMapper mapper
) : EndpointWithoutRequest<List<TaskResponse>>
{
    private readonly ITaskService _taskService = taskService;
    public override void Configure()
    {
        Get("/tasks");
        AllowAnonymous();
    }

    public override async Task<List<TaskResponse>> ExecuteAsync(CancellationToken ct)
    {
        var result = await _taskService.GetTasksAsync();
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);
            
        var response = result.Value.Select(mapper.Map<TaskResponse>).ToList();
        return response;
    }
}
