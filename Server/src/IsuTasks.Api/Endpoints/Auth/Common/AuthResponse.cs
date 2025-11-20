namespace IsuTasks.Api.Endpoints.Auth.Common;

public record AuthResponse(
    string AccessToken,
    string RefreshToken
);
