using Library.Services.Books.Models;

namespace Library.Services.Books
{
    public interface IBooksService
    {
        AllBooksServiceModel GetAllBooks(int currentPage);
    }
}
