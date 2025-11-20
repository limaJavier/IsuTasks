using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Auth.Common;
using IsuTasks.Api.Services.Auth;
using IsuTasks.Api.Utils;

namespace IsuTasks.Api.Endpoints.Auth.Logout;

public class LogoutEndpoint(IAuthService authService) : Endpoint<LogoutRequest>
{
    private readonly IAuthService _authService = authService;

    public override void Configure()
    {
        Post("/auth/logout");
    }

    public override async Task HandleAsync(LogoutRequest request, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Unexpected("Cannot resolve user-id from access-token");

        var result = await _authService.LogoutAsync(userId, request.RefreshToken);
        if(result.IsFailure)
            throw ApiException.FromError(result.Error);

        await Send.OkAsync();
    }
}
