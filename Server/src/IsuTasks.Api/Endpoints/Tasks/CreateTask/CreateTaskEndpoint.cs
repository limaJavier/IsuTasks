using FastEndpoints;
using IsuTasks.Api.Domain.Entities;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Services.Tasks;
using IsuTasks.Api.Utils;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api.Endpoints.Tasks.CreateTask;

public class CreateTaskEndpoint(
    ITaskService taskService,
    IMapper mapper
) : Endpoint<CreateTaskRequest>
{
    private readonly ITaskService _taskService = taskService;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Post("/tasks");
    }

    public override async Task HandleAsync(CreateTaskRequest request, CancellationToken ct)
    {
        // TODO: Implement validators (all of them)
        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Unexpected("Cannot resolve user-id from access-token");
        
        var task = _mapper.Map<IsuTask>((request, userId));
        var result = await _taskService.CreateTaskAsync(task);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        await Send.CreatedAtAsync<CreateTaskEndpoint>();
    }
}
