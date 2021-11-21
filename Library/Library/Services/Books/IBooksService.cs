using System.Collections.Generic;
using Library.Services.Books.Models;

namespace Library.Services.Books
{
    public interface IBooksService
    {
        AllBooksServiceModel GetAllBooks(int currentPage);

        BookDetailsServiceModel GetBookDetails(string bookId, string userId);

        IEnumerable<GenreServiceModel> GetAllGenresServiceModels();

        AllBooksServiceModel GetMyLibrary(int currentPage, string userId);

        bool AddBookAndReturnBoolean
            (AddBookFormModel addBookFormModel, string userId);
    }
}
