using FastEndpoints;
using FastEndpoints.Swagger;
using IsuTasks.Api.Persistence;
using IsuTasks.Api.Utils;
using Microsoft.EntityFrameworkCore;

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
        app.ApplyMigrations();

        return app;
    }

    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IsuTasksDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}
