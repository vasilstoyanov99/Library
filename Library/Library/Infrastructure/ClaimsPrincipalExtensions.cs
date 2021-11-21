namespace Library.Infrastructure
{
    using System.Security.Claims;

    using static Areas.Admin.AdminConstants;
    using static Areas.User.UserConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdminRoleName);

        public static bool IsUser(this ClaimsPrincipal user)
            => user.IsInRole(UserRoleName);
    }
}