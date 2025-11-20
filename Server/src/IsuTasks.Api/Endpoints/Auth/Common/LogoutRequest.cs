namespace IsuTasks.Api.Endpoints.Auth.Common;

public record LogoutRequest(
    string RefreshToken
);
