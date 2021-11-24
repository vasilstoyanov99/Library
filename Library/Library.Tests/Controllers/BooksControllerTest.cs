using System.Linq;
using Library.Data.Models;
using Shouldly;

namespace Library.Tests.Controllers
{
    using System.Collections.Generic;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Library.Controllers;
    using Library.Services.Books.Models;

    using static Data.DbModels.BooksControllerTestDbModels;
    using static Library.Global.GlobalConstants.Notifications;
    using static Library.Areas.User.UserConstants;
    using static Library.Areas.Admin.AdminConstants;
    public class BooksControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithModelWithValidData()
        {
            var expectedModel = AllBooksModel;

            MyController<BooksController>
                .Instance()
                .WithData(TestBook)
                .Calling(c => c.All(1))
                .ShouldReturn()
                .View(expectedModel);
        }

        [Fact]
        public void DetailsShouldReturnViewWithModelWithValidDataAndUserShouldBeAbleToEdit()
        {
            var expectedModel = BookDetailsModel;
            expectedModel.CanUserEdit = true;

            MyController<BooksController>
                .Instance()
                .WithUser()
                .WithData(TestGenre, TestBook)
                .Calling(c => c.Details(TestBook.Id))
                .ShouldReturn()
                .View(expectedModel);
        }

        [Fact]
        public void DetailsShouldReturnViewWithModelWithValidDataAndUserShouldNotBeAbleToEdit()
        {
            var expectedModel = BookDetailsModel;
            expectedModel.CanUserEdit = false;

            MyController<BooksController>
                .Instance()
                .WithData(TestGenre, TestBook)
                .Calling(c => c.Details(TestBook.Id))
                .ShouldReturn()
                .View(expectedModel);
        }

        [Fact]
        public void GetAddBookMethodShouldAuthorizeFilter()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.AddBook())
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());
        }

        [Fact]
        public void AddBookShouldReturnView()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.AddBook())
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void PostAddBookMethodShouldAuthorizeFilter()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.AddBook(new AddBookFormModel()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());
        }

        [Fact]
        public void AddBookShouldRedirectWithTempDataMessageAndSaveBookWithValidData()
        {
            var expectedModel = AddBookModel;

            MyController<BooksController>
                .Instance()
                .WithUser(u => u.InRole(UserRoleName))
                .WithData(TestGenre)
                .Calling(c => c.AddBook(expectedModel))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Book>(set =>
                    {
                        set.ShouldNotBeNull();
                        set
                            .FirstOrDefault(s => s.GenreId == TestGenre.Id)
                            .ShouldNotBeNull();
                        set.FirstOrDefault(s => s.UserId == TestBook.UserId)
                            .ShouldNotBeNull();
                    }))
                .AndAlso()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SuccessfullyAddedBookKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("MyLibrary");
        }

        [Fact]
        public void EditBookMethodShouldAuthorizeFilter()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.EditBook(null))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());
        }

        [Fact]
        public void EditBookShouldReturnView()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.EditBook(null))
                .ShouldReturn() 
                .View();
        }

        [Fact]
        public void PostEditMethodShouldAuthorizeFilter()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.Edit(new EditBookFormModel()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());
        }

        [Fact]
        public void EditShouldRedirectWithTempDataMessageAndSaveEditedBookWithValidData()
        {
            var editedModel = EditedBookModel;

            MyController<BooksController>
                .Instance()
                .WithUser(u => u.InRole(UserRoleName))
                .WithData(TestBook, TestGenre)
                .Calling(c => c.Edit(editedModel))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Book>(set =>
                    {
                        var newTitle = set.First().Title;
                        newTitle.ShouldNotBeSameAs(TestBook.Title);
                    }))
                .AndAlso()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SuccessfullyEditedBookKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");
        }

        [Fact]
        public void DeleteMethodShouldAuthorizeFilter()
        {
            MyController<BooksController>
                .Instance()
                .Calling(c => c.Delete(null))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());
        }

        [Fact]
        public void DeleteShouldRedirectWithTempDataMessageAndDeleteTheGivenBookByValidId()
        {
            MyController<BooksController>
                .Instance()
                .WithUser(u => u.InRole(UserRoleName))
                .WithData(TestBook)
                .Calling(c => c.Delete(TestBook.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Book>(set => set.ShouldBeEmpty()))
                .AndAlso()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SuccessfullyDeletedBookKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All");
        }

        [Fact]
        public void MyLibraryShouldReturnViewWithModelWithValidData()
        {
            var expectedModel = AllBooksModel;

            MyController<BooksController>
                .Instance()
                .WithUser()
                .WithData(TestBook)
                .Calling(c => c.MyLibrary(1))
                .ShouldReturn()
                .View(expectedModel);
        }
    }
}
