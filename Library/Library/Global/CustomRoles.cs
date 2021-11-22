namespace Library.Global
{
    using static Areas.Admin.AdminConstants;
    using static Areas.User.UserConstants;

    public static class CustomRoles
    {
        public const string AdminOrUser = AdminRoleName + "," + UserRoleName;
    }
}
