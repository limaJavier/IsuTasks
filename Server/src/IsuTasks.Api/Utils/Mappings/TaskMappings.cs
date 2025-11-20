using IsuTasks.Api.Domain.Entities;
using IsuTasks.Api.Endpoints.Tasks.CreateTask;
using Mapster;

namespace IsuTasks.Api.Utils.Mappings;

public class TaskMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateTaskRequest Request, Guid UserId), IsuTask>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);
    }
}
