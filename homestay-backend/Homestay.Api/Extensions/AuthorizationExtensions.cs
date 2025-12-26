using Homestay.Api.Authorization.Policies;
using Homestay.Api.Authorization.Requirements;
using Homestay.Api.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace Homestay.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
                policy.RequireRole("admin"));

            options.AddPolicy(AuthorizationPolicies.HostOnly, policy =>
                policy.RequireRole("host"));

            options.AddPolicy(AuthorizationPolicies.GuestOnly, policy =>
                policy.RequireRole("guest"));

            options.AddPolicy(AuthorizationPolicies.HostOrAdmin, policy =>
                policy.RequireRole("host", "admin"));

            options.AddPolicy(AuthorizationPolicies.ResourceOwner, policy =>
                policy.Requirements.Add(new ResourceOwnerRequirement()));
        });

        services.AddSingleton<IAuthorizationHandler, ResourceOwnerHandler>();

        return services;
    }
}
