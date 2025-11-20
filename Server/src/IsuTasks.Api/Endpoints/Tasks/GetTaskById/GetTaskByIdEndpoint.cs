using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Tasks.Common;
using IsuTasks.Api.Services.Tasks;
using IsuTasks.Api.Utils;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api.Endpoints.Tasks.GetTaskById;

public class GetTaskByIdEndpoint(
    ITaskService taskService,
    IMapper mapper
) : EndpointWithoutRequest<TaskResponse>
{
    private readonly ITaskService _taskService = taskService;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Get("/tasks/{id:guid}");
    }

    public override async Task<TaskResponse> ExecuteAsync(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Unexpected("Cannot resolve user-id from access-token");

        var id = Route<Guid>("id");

        var result = await _taskService.GetTaskByIdAsync(id, userId);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        var response = _mapper.Map<TaskResponse>(result.Value);
        return response;
    }
}
