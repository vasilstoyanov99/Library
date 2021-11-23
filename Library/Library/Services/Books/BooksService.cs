﻿using Ganss.XSS;

namespace Library.Services.Books
{
    using System.Collections.Generic;
    using Data;
    using Library.Data.Models;
    using Models;
    using System;
    using System.Linq;
    using static Global.GlobalConstants.Paging;

    public class BooksService : IBooksService
    {
        private readonly LibraryDbContext _data;

        public BooksService(LibraryDbContext data) => _data = data;

        public AllBooksServiceModel GetAllBooks(int currentPage)
        {
            var booksQuery = _data.Books.AsQueryable();
            var booksCount = booksQuery.Count();
            var maxPage = GetMaxPage(booksCount);

            currentPage =
                CheckIfCurrentPageIsOutOfRangeAndReturnCurrentPageStart
                    (currentPage, maxPage);

            return GetAllBooksQueryModel(currentPage, maxPage, booksQuery);
        }

        public BookDetailsServiceModel GetBookDetails(string bookId, string userId)
        {
            var book = GetBookFromDb(bookId);

            if (book == null)
                return null;

            var genre = GetGenreFromDb(book.GenreId).Name;
            var canUserEdit = book.UserId == userId;

            return GetBookDetailsServiceModel(book, genre, canUserEdit);
        }

        public List<GenreServiceModel> GetAllGenresServiceModels()
            => _data
                .Genres
                .Select(g => new GenreServiceModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToList();

        public AllBooksServiceModel GetMyLibrary(int currentPage, string userId)
        {
            var booksQuery = _data
                .Books
                .Where(b => b.UserId == userId)
                .AsQueryable();
            var booksCount = booksQuery.Count();
            var maxPage = GetMaxPage(booksCount);

            currentPage = 
                CheckIfCurrentPageIsOutOfRangeAndReturnCurrentPageStart
                    (currentPage, maxPage);

            return GetAllBooksQueryModel(currentPage, maxPage, booksQuery);
        }

        public (bool doesTitleExistsInDb, bool genreDoesNotExistsInDb) AddBookAndReturnBooleans
            (AddBookFormModel addBookFormModel, string userId)
        {
            var doesTitleExistsInDb = false;
            var genreDoesNotExistsInDb = false;
            var htmlSanitizer = new HtmlSanitizer();
            var title = htmlSanitizer.Sanitize(addBookFormModel.Title).Trim();

            if (_data.Books.Any(b => b.Title == title))
            {
                doesTitleExistsInDb = true;
                return (doesTitleExistsInDb, genreDoesNotExistsInDb);
            }

            var genreId = htmlSanitizer.Sanitize(addBookFormModel.GenreId);

            if (!CheckIfGenreExistsInDb(genreId))
            {
                genreDoesNotExistsInDb = true;
                return (doesTitleExistsInDb, genreDoesNotExistsInDb);
            }

            var newBook = FillBookDbModelWithDataAndReturnIt
                (addBookFormModel, genreId, title, userId, htmlSanitizer);
            _data.Books.Add(newBook);
            _data.SaveChanges();

            return (doesTitleExistsInDb, doesTitleExistsInDb);
        }

        public EditBookFormModel GetEditBookFormModel(string bookId)
        {
            var book = GetBookFromDb(bookId);

            if (book == null)
                return null;

            var genre = GetGenreFromDb(book.GenreId).Name;

            return GetEditBookFormModel(book, genre);
        }

        public (bool bookDoesNotExistsInDb, bool genreDoesNotExistsInDb) 
            EditBookAndReturnBooleans(EditBookFormModel editBookFormModel)
        {
            var bookDoesNotExistsInDb = false;
            var genreDoesNotExistsInDb = false;

            var book = GetBookFromDb(editBookFormModel.Id);

            if (book == null)
            {
                bookDoesNotExistsInDb = true;
                return (bookDoesNotExistsInDb, genreDoesNotExistsInDb);
            }

            var htmlSanitizer = new HtmlSanitizer();
            var genreId = htmlSanitizer.Sanitize(editBookFormModel.GenreId);

            if (!CheckIfGenreExistsInDb(genreId))
            {
                genreDoesNotExistsInDb = true;
                return (bookDoesNotExistsInDb, genreDoesNotExistsInDb);
            }

            UpdateBookDbModelAndSaveChanges(book, genreId, editBookFormModel, htmlSanitizer);

            return (bookDoesNotExistsInDb, genreDoesNotExistsInDb);
        }

        public bool DeleteBookAndReturnBoolean(string bookId)
        {
            var book = GetBookFromDb(bookId);

            if (book == null)
                return false;

            _data.Books.Remove(book);
            _data.SaveChanges();

            return true;
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
        (AddBookFormModel addBookFormModel, string genreId, string title,
            string userId, HtmlSanitizer htmlSanitizer) =>
            new()
            {
                Title = title,
                Author = htmlSanitizer.Sanitize(addBookFormModel.Author).Trim(),
                GenreId = genreId,
                ShortDescription = htmlSanitizer.Sanitize
                    (addBookFormModel.ShortDescription).Trim(),
                LongDescription = htmlSanitizer.Sanitize
                    (addBookFormModel.LongDescription).Trim(),
                ImageUrl = htmlSanitizer.Sanitize(addBookFormModel.ImageUrl).Trim(),
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
                Genres = GetAllGenresServiceModels()
            };

        private void UpdateBookDbModelAndSaveChanges
            (Book book, string genreId, 
                EditBookFormModel editBookFormModel, HtmlSanitizer htmlSanitizer)
        {
            book.Author = htmlSanitizer.Sanitize(editBookFormModel.Author).Trim();
            book.Title = htmlSanitizer.Sanitize(editBookFormModel.Title).Trim();
            book.GenreId = genreId;
            book.ShortDescription = htmlSanitizer.Sanitize
                (editBookFormModel.ShortDescription).Trim();
            book.LongDescription = htmlSanitizer.Sanitize
                (editBookFormModel.LongDescription).Trim();
            book.ImageUrl = htmlSanitizer.Sanitize(editBookFormModel.ImageUrl).Trim();
            _data.SaveChanges();
        }

        private bool CheckIfGenreExistsInDb(string genreId)
        {
            var genre = GetGenreFromDb(genreId);

            return genre != null;
        }

        private Book GetBookFromDb(string bookId) =>
            _data
                .Books
                .FirstOrDefault(b => b.Id == bookId);

        private Genre GetGenreFromDb(string genreId) =>
            _data
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
