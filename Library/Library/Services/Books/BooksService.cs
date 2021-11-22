using Ganss.XSS;

namespace Library.Services.Books
{
    using System.Collections.Generic;
    using Library.Data;
    using Library.Data.Models;
    using Library.Services.Books.Models;
    using System;
    using System.Linq;
    using static Global.GlobalConstants.Paging;

    public class BooksService : IBooksService
    {
        private readonly LibraryDbContext _data;

        public BooksService(LibraryDbContext data) => this._data = data;

        public AllBooksServiceModel GetAllBooks(int currentPage)
        {
            var booksQuery = this._data.Books.AsQueryable();
            var booksCount = booksQuery.Count();
            var maxPage = this.GetMaxPage(booksCount);

            currentPage =
                this.CheckIfCurrentPageIsOutOfRangeAndReturnCurrentPageStart
                    (currentPage, maxPage);

            return GetAllBooksQueryModel(currentPage, maxPage, booksQuery);
        }

        public BookDetailsServiceModel GetBookDetails(string bookId, string userId)
        {
            var book = this.GetBookFromDb(bookId);

            if (book == null)
                return null;

            var genre = this.GetGenreFromDb(book.GenreId).Name;
            var canUserEdit = book.UserId == userId;

            return GetBookDetailsServiceModel(book, genre, canUserEdit);
        }

        public List<GenreServiceModel> GetAllGenresServiceModels()
            => this._data
                .Genres
                .Select(g => new GenreServiceModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToList();

        public AllBooksServiceModel GetMyLibrary(int currentPage, string userId)
        {
            var booksQuery = this._data
                .Books
                .Where(b => b.UserId == userId)
                .AsQueryable();
            var booksCount = booksQuery.Count();
            var maxPage = this.GetMaxPage(booksCount);

            currentPage = 
                this.CheckIfCurrentPageIsOutOfRangeAndReturnCurrentPageStart
                    (currentPage, maxPage);

            return GetAllBooksQueryModel(currentPage, maxPage, booksQuery);
        }

        public (bool doesTitleExistsInDb, bool genreDoesNotExistsInDb) AddBookAndReturnBooleans
            (AddBookFormModel addBookFormModel, string userId)
        {
            var doesTitleExistsInDb = false;
            var genreDoesNotExistsInDb = false;

            if (this._data.Books.Any(b => b.Title == addBookFormModel.Title))
            {
                doesTitleExistsInDb = true;
                return (doesTitleExistsInDb, genreDoesNotExistsInDb);
            }

            var htmlSanitizer = new HtmlSanitizer();
            var genreId = htmlSanitizer.Sanitize(addBookFormModel.GenreId);

            if (!this.CheckIfGenreExistsInDb(genreId))
            {
                genreDoesNotExistsInDb = true;
                return (doesTitleExistsInDb, genreDoesNotExistsInDb);
            }

            var newBook = this.FillBookDbModelWithDataAndReturnIt
                (addBookFormModel, genreId, userId, htmlSanitizer);
            this._data.Books.Add(newBook);
            this._data.SaveChanges();

            return (doesTitleExistsInDb, doesTitleExistsInDb);
        }

        public EditBookFormModel GetEditBookFormModel(string bookId)
        {
            var book = this.GetBookFromDb(bookId);

            if (book == null)
                return null;

            var genre = GetGenreFromDb(book.GenreId).Name;

            return this.GetEditBookFormModel(book, genre);
        }

        public (bool bookDoesNotExistsInDb, bool genreDoesNotExistsInDb) 
            EditBookAndReturnBooleans(EditBookFormModel editBookFormModel)
        {
            var bookDoesNotExistsInDb = false;
            var genreDoesNotExistsInDb = false;

            var book = this.GetBookFromDb(editBookFormModel.Id);

            if (book == null)
            {
                bookDoesNotExistsInDb = true;
                return (bookDoesNotExistsInDb, genreDoesNotExistsInDb);
            }

            var htmlSanitizer = new HtmlSanitizer();
            var genreId = htmlSanitizer.Sanitize(editBookFormModel.GenreId);

            if (!this.CheckIfGenreExistsInDb(genreId))
            {
                genreDoesNotExistsInDb = true;
                return (bookDoesNotExistsInDb, genreDoesNotExistsInDb);
            }

            UpdateBookDbModelAndSaveChanges(book, genreId, editBookFormModel, htmlSanitizer);

            return (bookDoesNotExistsInDb, genreDoesNotExistsInDb);
        }

        private static AllBooksServiceModel GetAllBooksQueryModel
            (int currentPage, double maxPage, IQueryable<Book> booksQuery)
            => new()
            {
                CurrentPage = currentPage,
                MaxPage = maxPage,
                Books = GetAllBooksListingModels
                (booksQuery
                    .Skip((currentPage - 1) * ThreeCardsPerPage)
                    .Take(ThreeCardsPerPage))
            };

        private static IEnumerable<BookListingServiceModel> GetAllBooksListingModels
            (IQueryable<Book> booksQuery) =>
            booksQuery
                .Select(b => new BookListingServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription
                })
                .OrderBy(b => b.Title)
                .ToList();

        private static BookDetailsServiceModel
            GetBookDetailsServiceModel(Book book, string genre, bool canUserEdit)
            => new()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = genre,
                ImageUrl = book.ImageUrl,
                LongDescription = book.LongDescription,
                CanUserEdit = canUserEdit
            };

        private Book FillBookDbModelWithDataAndReturnIt
        (AddBookFormModel addBookFormModel, string genreId,
            string userId, HtmlSanitizer htmlSanitizer) =>
            new()
            {
                Title = htmlSanitizer.Sanitize(addBookFormModel.Title),
                Author = htmlSanitizer.Sanitize(addBookFormModel.Author),
                GenreId = genreId,
                ShortDescription = htmlSanitizer.Sanitize
                    (addBookFormModel.ShortDescription),
                LongDescription = htmlSanitizer.Sanitize
                    (addBookFormModel.LongDescription),
                ImageUrl = htmlSanitizer.Sanitize(addBookFormModel.ImageUrl),
                UserId = userId
            };

        private EditBookFormModel GetEditBookFormModel
            (Book book, string genre) =>
            new()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CurrentGenre = genre,
                GenreId = book.GenreId,
                ImageUrl = book.ImageUrl,
                LongDescription = book.LongDescription,
                ShortDescription = book.ShortDescription,
                Genres = this.GetAllGenresServiceModels()
            };

        private void UpdateBookDbModelAndSaveChanges
            (Book book, string genreId, 
                EditBookFormModel editBookFormModel, HtmlSanitizer htmlSanitizer)
        {
            book.Author = htmlSanitizer.Sanitize(editBookFormModel.Author);
            book.Title = htmlSanitizer.Sanitize(editBookFormModel.Title);
            book.GenreId = genreId;
            book.ShortDescription = htmlSanitizer.Sanitize(editBookFormModel.ShortDescription);
            book.LongDescription = htmlSanitizer.Sanitize(editBookFormModel.LongDescription);
            book.ImageUrl = htmlSanitizer.Sanitize(editBookFormModel.ImageUrl);
            this._data.SaveChanges();
        }

        private bool CheckIfGenreExistsInDb(string genreId)
        {
            var genre = this.GetGenreFromDb(genreId);

            return genre != null;
        }

        private Book GetBookFromDb(string bookId) =>
            this._data
                .Books
                .FirstOrDefault(b => b.Id == bookId);

        private Genre GetGenreFromDb(string genreId) =>
            this._data
                .Genres
                .FirstOrDefault(g => g.Id == genreId);

        private double GetMaxPage(int count) =>
            Math.Ceiling
                (count * 1.00 / ThreeCardsPerPage * 1.00);

        private int
            CheckIfCurrentPageIsOutOfRangeAndReturnCurrentPageStart
            (int currentPage, double maxPage)
        {
            if (currentPage > maxPage || currentPage < 1)
            {
                return CurrentPageStart;
            }
            else
            {
                return currentPage;
            }
        }
    }
}
