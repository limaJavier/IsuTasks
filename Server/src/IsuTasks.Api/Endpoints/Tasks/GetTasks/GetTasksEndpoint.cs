using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Tasks.Common;
using IsuTasks.Api.Services.Tasks;
using IsuTasks.Api.Utils;
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
    }

    public override async Task<List<TaskResponse>> ExecuteAsync(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Unexpected("Cannot resolve user-id from access-token");

        var result = await _taskService.GetTasksAsync(userId);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);
            
        var response = result.Value.Select(mapper.Map<TaskResponse>).ToList();
        return response;
    }
}
