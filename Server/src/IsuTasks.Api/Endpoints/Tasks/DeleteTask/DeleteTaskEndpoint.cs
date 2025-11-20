using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Services.Tasks;

namespace IsuTasks.Api.Endpoints.Tasks.DeleteTask;

public class DeleteClassEndpoint(ITaskService taskService) : EndpointWithoutRequest
{
    private readonly ITaskService _taskService = taskService;

    public override void Configure()
    {   
        Delete("/tasks/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await _taskService.DeleteTaskAsync(id);
        if(result.IsFailure)
            throw ApiException.FromError(result.Error);

        await Send.OkAsync();
    }
}
