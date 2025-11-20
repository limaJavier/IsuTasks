using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Auth.Common;
using IsuTasks.Api.Services.Auth;
using IsuTasks.Api.Utils;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api.Endpoints.Auth.Refresh;

public class RefreshEndpoint(
    IAuthService authService,
    IMapper mapper
) : Endpoint<RefreshRequest, AuthResponse>
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Post("/auth/refresh");
    }

    public override async Task<AuthResponse> ExecuteAsync(RefreshRequest request, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId()
            ?? throw ApiException.Conflict("Cannot resolve user-id from access-token");

        var result = await _authService.RefreshAsync(userId, request.RefreshToken);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        var response = _mapper.Map<AuthResponse>(result.Value);
        return response;
    }
}
