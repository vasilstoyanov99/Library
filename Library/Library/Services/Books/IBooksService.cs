using System.Collections.Generic;
using Library.Services.Books.Models;

namespace Library.Services.Books
{
    public interface IBooksService
    {
        AllBooksServiceModel GetAllBooks(int currentPage);

        BookDetailsServiceModel GetBookDetails(string bookId, string userId);

        List<GenreServiceModel> GetAllGenresServiceModels();

        AllBooksServiceModel GetMyLibrary(int currentPage, string userId);

        (bool doesTitleExistsInDb, bool genreDoesNotExistsInDb) AddBookAndReturnBooleans
            (AddBookFormModel addBookFormModel, string userId);

        EditBookFormModel GetEditBookFormModel(string bookId);

        (bool bookDoesNotExistsInDb, bool genreDoesNotExistsInDb) 
            EditBookAndReturnBooleans(EditBookFormModel editBookFormModel);
    }
}
