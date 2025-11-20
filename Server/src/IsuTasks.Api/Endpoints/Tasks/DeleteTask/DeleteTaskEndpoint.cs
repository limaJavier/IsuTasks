using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Services.Tasks;
using IsuTasks.Api.Utils;

namespace IsuTasks.Api.Endpoints.Tasks.DeleteTask;

public class DeleteClassEndpoint(ITaskService taskService) : EndpointWithoutRequest
{
    private readonly ITaskService _taskService = taskService;

    public override void Configure()
    {
        Delete("/tasks/{id:guid}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Unexpected("Cannot resolve user-id from access-token");

        var result = await _taskService.DeleteTaskAsync(id, userId);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        await Send.OkAsync();
    }
}
