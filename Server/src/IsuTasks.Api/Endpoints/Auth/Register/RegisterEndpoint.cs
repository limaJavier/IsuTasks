using FastEndpoints;
using IsuTasks.Api.Domain.Exceptions;
using IsuTasks.Api.Endpoints.Auth.Common;
using IsuTasks.Api.Services.Auth;
using IMapper = MapsterMapper.IMapper;

namespace IsuTasks.Api.Endpoints.Auth.Register;

public class RegisterEndpoint(
    IAuthService authService,
    IMapper mapper
) : Endpoint<AuthRequest, AuthResponse>
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();
    }

    public override async Task<AuthResponse> ExecuteAsync(AuthRequest request, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(request.Email, request.Password);
        if (result.IsFailure)
            throw ApiException.FromError(result.Error);

        var response = _mapper.Map<AuthResponse>(result.Value);
        return response;
    }
}

