namespace Library.Tests.Routing
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Library.Controllers;

    public class BooksControllerTest
    {
        [Fact]
        public void GetAllShouldBeMappedWithRouteKey()
            => MyRouting
                .Configuration()
                .ShouldMap("/Books/All?currentPage=0")
                .To<BooksController>(b => b.All(0));

        [Fact]
        public void GetDetailsShouldBeMappedWithRouteKey()
            => MyRouting
                .Configuration()
                .ShouldMap("/Books/Details?bookId=0")
                .To<BooksController>(b => b.Details("0"));

        [Fact]
        public void GetMyLibraryShouldBeMappedWithRouteKey()
            => MyRouting
                .Configuration()
                .ShouldMap("/Books/MyLibrary?currentPage=0")
                .To<BooksController>(b => b.MyLibrary(0));

        [Fact]
        public void GetAddBookShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Books/AddBook")
                .To<BooksController>(b => b.AddBook());

        [Fact]
        public void PostAddBookShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Books/AddBook")
                    .WithMethod(HttpMethod.Post))
                .To<BooksController>(b => b.AddBook(null));

        [Fact]
        public void GetEditBookShouldBeMappedWithRouteKey()
            => MyRouting
                .Configuration()
                .ShouldMap("/Books/EditBook?bookId=0")
                .To<BooksController>(b => b.EditBook("0"));

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Books/Edit")
                .WithMethod(HttpMethod.Post))
                .To<BooksController>(b => b.Edit(null));

        [Fact]
        public void GetDeleteShouldBeMappedWithRouteKey()
            => MyRouting
                .Configuration()
                .ShouldMap("/Books/Delete?bookId=0")
                .To<BooksController>(b => b.Delete("0"));
    }
}
