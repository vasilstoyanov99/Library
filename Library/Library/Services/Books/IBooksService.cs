namespace Library.Services.Books
{
    using System.Collections.Generic;
    using Models;

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

        bool DeleteBookAndReturnBoolean(string bookId);
    }
}
