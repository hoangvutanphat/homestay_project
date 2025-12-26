using Microsoft.AspNetCore.Authorization;

namespace Homestay.Api.Authorization.Requirements;

public class ResourceOwnerRequirement : IAuthorizationRequirement
{
    public ResourceOwnerRequirement(){}
}
