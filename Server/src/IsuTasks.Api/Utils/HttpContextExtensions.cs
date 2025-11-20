using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IsuTasks.Api.Utils;

public static class HttpContextExtensions
{
    private const string accessToken = "access_token";

    public static string GetTraceId(this HttpContext httpContext) => Activity.Current?.Id ?? httpContext.TraceIdentifier;

    public static string? GetAccessToken(this HttpContext httpContext) => httpContext.GetTokenAsync(accessToken).Result;

    public static Guid? GetUserId(this HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim is null ? null : Guid.Parse(userIdClaim.Value);
    }    
}
