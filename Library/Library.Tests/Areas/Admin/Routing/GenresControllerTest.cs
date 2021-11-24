namespace Library.Tests.Areas.Admin.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using Library.Areas.Admin.Controllers;
    public class GenresControllerTest
    {
        [Fact]
        public void GetAddGenreShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Genres/AddGenre")
                .To<GenresController>(g => g.AddGenre());

        [Fact]
        public void PostAddGenreShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Genres/AddGenre")
                    .WithMethod(HttpMethod.Post))
                .To<GenresController>(b => b.AddGenre(null));
    }
}
