namespace IsuTasks.Api.Endpoints.Auth.Common;

public record AuthRequest(
    string Email,
    string Password
);
