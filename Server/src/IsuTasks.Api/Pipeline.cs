using FastEndpoints;
using FastEndpoints.Swagger;
using IsuTasks.Api.Utils;

namespace IsuTasks.Api;

public static class Pipeline
{
    public static IApplicationBuilder ConfigurePipeline(this IApplicationBuilder app)
    {
        app.UseCors(DependencyInjection.CorsAllowAnyOriginPolicy);
        app.UseExceptionHandler(app => app.Run(GlobalExceptionHandler.Handle));
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app
            .UseFastEndpoints()
            .UseSwaggerGen();

        return app;
    }
}
