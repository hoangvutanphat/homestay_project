namespace Homestay.Api.Authorization.Policies;
public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string HostOnly = "HostOnly";
    public const string GuestOnly = "GuestOnly";
    public const string HostOrAdmin = "HostOrAdmin";
    public const string ResourceOwner = "ResourceOwner";
}
