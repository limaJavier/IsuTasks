using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using IsuTasks.Api.Persistence;
using IsuTasks.Api.Services.Auth;
using IsuTasks.Api.Services.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api;

public static class DependencyInjection
{
    private const string connectionStringKey = "IsuTask";
    public const string CorsAllowAnyOriginPolicy = "CorsAllowAnyOriginPolicy";

    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors();
        services.AddAuth();
        services.AddServices(configuration);
        services.AddMappings();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IsuTasksDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(connectionStringKey)));

        services.AddFastEndpoints()
            .SwaggerDocument();

        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddSingleton<IHasher, Hasher>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "isu.task.auth",
                ValidAudience = "isu.task",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("An at least 256 bits signing key")), // TODO: Save credential info in a secure place
            };
        });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        // TODO: Explicitly define mappings
        config.Scan(
            typeof(DependencyInjection).Assembly
        );

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsAllowAnyOriginPolicy,
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        return services;
    }
}
