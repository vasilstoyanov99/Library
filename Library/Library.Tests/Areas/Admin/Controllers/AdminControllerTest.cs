namespace Library.Tests.Areas.Admin.Controllers
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

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
