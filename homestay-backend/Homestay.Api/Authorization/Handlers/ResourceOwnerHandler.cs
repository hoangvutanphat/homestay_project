using System.Security.Claims;
using Homestay.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Homestay.Api.Authorization.Handlers;

public class ResourceOwnerHandler : AuthorizationHandler<ResourceOwnerRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResourceOwnerHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ResourceOwnerRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return Task.CompletedTask;
        }
    
        var routeData = httpContext.GetRouteData();
        var ownerId = routeData.Values["userId"]?.ToString();

        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole == "admin")
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (userId == ownerId)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
