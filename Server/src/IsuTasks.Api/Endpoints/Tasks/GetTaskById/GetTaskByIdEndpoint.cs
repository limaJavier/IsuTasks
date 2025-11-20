using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Tasks.Common;
using IsuTasks.Api.Services.Tasks;
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
        AllowAnonymous();
    }

    public override async Task<TaskResponse> ExecuteAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await _taskService.GetTaskByIdAsync(id);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        var response = _mapper.Map<TaskResponse>(result.Value);
        return response;
    }
}
