using System.Collections.Generic;
using Library.Services.Books.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Tests.Data.DbModels
{
    using Library.Data.Models;

    public class BooksControllerTestDbModels
    {
        public static Book TestBook => new()
        {
            Id = "TestBookId",
            Author = "TestAuthor",
            Title = "TestTitle",
            GenreId = TestGenre.Id,
            ImageUrl = "https://i.imgur.com/OTLkRq4.jpg",
            LongDescription =
                "Morbi convallis ex turpis, sit amet imperdiet nisl fringilla eget. Integer malesuada vitae nisl quis feugiat. Aenean eget magna eget libero finibus elementum. Aenean tincidunt elit sit amet felis egestas viverra. Fusce sit amet urna est. Vivamus eu tellus ac felis tempus pretium vel sit amet ante.",
            ShortDescription =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sollicitudin facilisis fringilla.",
            UserId = "TestId"
        };

        public static Genre TestGenre => new()
        {
            Id = "TestGenreId",
            Name = "TestGenre"
        };

        public static AllBooksServiceModel AllBooksModel = new()
        {
            CurrentPage = 1,
            MaxPage = 1,
            Books = new List<BookListingServiceModel>()
            {
                new()
                {
                    Id = TestBook.Id,
                    Author = TestBook.Author,
                    Title = TestBook.Title,
                    ShortDescription = TestBook.ShortDescription,
                    ImageUrl = TestBook.ImageUrl
                }
            }
        };

        public static BookDetailsServiceModel BookDetailsModel = new()
        {
            Id = TestBook.Id,
            Author = TestBook.Author,
            Title = TestBook.Title,
            ImageUrl = TestBook.ImageUrl,
            LongDescription = TestBook.LongDescription,
            Genre = TestGenre.Name
        };

        public static AddBookFormModel AddBookModel = new()
        {
            Author = TestBook.Author,
            Title = TestBook.Title,
            GenreId = TestBook.GenreId,
            ShortDescription = TestBook.ShortDescription,
            LongDescription = TestBook.LongDescription,
            ImageUrl = TestBook.ImageUrl
        };

        public static EditBookFormModel EditedBookModel = new()
        {
            Id = TestBook.Id,
            Title = "Test Book New Title",
            Author = TestBook.Author,
            CurrentGenre = TestGenre.Name,
            GenreId = TestBook.GenreId,
            ShortDescription = TestBook.ShortDescription,
            LongDescription = TestBook.LongDescription,
            ImageUrl = TestBook.ImageUrl
        };
    }
}
