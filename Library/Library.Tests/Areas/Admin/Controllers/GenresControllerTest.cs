namespace Library.Tests.Areas.Admin.Controllers
{
    using System.Linq;

    using Xunit;
    using Shouldly;
    using MyTested.AspNetCore.Mvc;

    using Library.Data.Models;
    using Library.Areas.Admin.Controllers;

    using static Data.DbModels.GenresControllerTestDbModels;
    using static Library.Global.GlobalConstants.Notifications;
    using static Library.Areas.Admin.AdminConstants;

    public class GenresControllerTest
    {
        [Fact]
        public void AddGenreShouldReturnView()
        {
            MyController<GenresController>
                .Instance()
                .Calling(c => c.AddGenre())
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void AddGenreShouldRedirectWithTempDataMessageAndSaveGenreWithValidData()
        {
            var addGenreFormModel = AddGenreModel;

            MyController<GenresController>
                .Instance()
                .WithUser(u => u.InRole(AdminRoleName))
                .Calling(c => c.AddGenre(addGenreFormModel))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Genre>(set =>
                    {
                        set.ShouldNotBeNull();
                        set.FirstOrDefault(g => g.Name == addGenreFormModel.Name)
                            .ShouldNotBeNull();
                    }))
                .AndAlso()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SuccessfullyAddedGenreKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("AddGenre");
        }
    }
}
