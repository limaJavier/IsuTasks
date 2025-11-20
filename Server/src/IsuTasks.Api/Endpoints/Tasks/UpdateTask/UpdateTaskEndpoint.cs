using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Services.Tasks;
using IsuTasks.Api.Utils;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api.Endpoints.Tasks.UpdateTask;

public class UpdateTaskEndpoint(
    ITaskService taskService,
    IMapper mapper
) : Endpoint<UpdateTaskRequest>
{
    private readonly ITaskService _taskService = taskService;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Put("/tasks");
    }

    public override async Task HandleAsync(UpdateTaskRequest request, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Unexpected("Cannot resolve user-id from access-token");

        var queryResult = await _taskService.GetTaskByIdAsync(request.Id, userId);
        if (queryResult.IsFailure)
            throw ApiException.FromError(queryResult.Error);

        _mapper.Map(request, queryResult.Value); // Update entity

        var updateResult = await _taskService.UpdateTaskAsync(queryResult.Value, userId);
        if(updateResult.IsFailure)
            throw ApiException.FromError(updateResult.Error);

        await Send.OkAsync();
    }
}
