namespace Library.Data.Seeding
{
    using System;
    using System.Linq;

    using Models;

    using static Data.UserSeedData;
    using static Data.BooksSeedData;

    public class BooksSeeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            if (!data.Books.Any(b => b.Title == BookOne.Title))
            {
                var user1Id = GetUserId(User1Email, data);
                FillBookDbModelAndAddItInDb(data, BookOne.Author, BookOne.Title, BookOne.GenreName, user1Id,
                    BookOne.ShortDescription, BookOne.LongDescription, BookOne.ImageUrl);
                FillBookDbModelAndAddItInDb(data, BookTwo.Author, BookTwo.Title, BookTwo.GenreName, user1Id,
                    BookTwo.ShortDescription, BookTwo.LongDescription, BookTwo.ImageUrl);
                var user2Id = GetUserId(User2Email, data);
                FillBookDbModelAndAddItInDb(data, BookThree.Author, BookThree.Title, BookThree.GenreName, user2Id, BookThree.ShortDescription, BookThree.LongDescription, BookThree.ImageUrl);
                FillBookDbModelAndAddItInDb(data, BookFour.Author, BookFour.Title, BookFour.GenreName, user2Id,
                    BookFour.ShortDescription, BookFour.LongDescription, BookFour.ImageUrl);

                data.SaveChanges();
            }
        }

        private void FillBookDbModelAndAddItInDb
            (LibraryDbContext data, string author, string title, string genreName, string userId,
                string shortDescription, string longDescription, string imageUrl)
        {
            var book = new Book()
            {
                Author = author,
                Title = title,
                GenreId = GetGenreId(genreName, data),
                UserId = userId,
                ShortDescription = shortDescription,
                LongDescription = longDescription,
                ImageUrl = imageUrl
            };

            data.Books.Add(book);
        }

        private string GetGenreId(string name, LibraryDbContext data)
            => data
                .Genres
                .FirstOrDefault(g => g.Name == name).Id;

        private string GetUserId(string email, LibraryDbContext data)
            => data
                .Users
                .FirstOrDefault(u => u.Email == email).Id;

    }
}
