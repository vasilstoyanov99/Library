namespace Library.Tests.Areas.Admin.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using Library.Areas.Admin.Controllers;

    using static Library.Areas.Admin.AdminConstants;

    public class AdminControllerTest
    {
        [Fact]
        public void ControllerShouldHaveAuthorizeFilter()
            => MyMvc
                .Controller<AdminController>()
                .ShouldHave()
                .Attributes(attrs =>
                {
                    attrs.RestrictingForAuthorizedRequests();
                    attrs.SpecifyingArea(AdminAreaName);
                });
    }
}
