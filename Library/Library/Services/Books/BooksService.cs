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
            var maxPage = Math.Ceiling
                (booksCount * 1.00 / ThreeCardsPerPage * 1.00);

            if (currentPage > maxPage || currentPage < 1)
                currentPage = CurrentPageStart;

            return GetAllBooksQueryModel(currentPage, maxPage, booksQuery);
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
    }
}
