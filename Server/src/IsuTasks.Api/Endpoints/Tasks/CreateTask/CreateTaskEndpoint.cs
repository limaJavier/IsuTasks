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
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTaskRequest request, CancellationToken ct)
    {
        // var userId = HttpContext.GetUserId();
        var userId = Guid.Parse("CBB486E9-8101-406E-BD41-5255966997C5");

        var task = _mapper.Map<IsuTask>((request, userId));
        var result = await _taskService.CreateTaskAsync(task);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        await Send.CreatedAtAsync<CreateTaskEndpoint>();
    }
}
