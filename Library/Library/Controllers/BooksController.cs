using Library.Infrastructure;
using Library.Services.Books.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Library.Services.Books;
    using System.Linq;
    using static Global.GlobalConstants.ErrorMessages;
    using static Areas.User.UserConstants;

    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService) 
            => _booksService = booksService;

        public IActionResult All([FromQuery] int currentPage)
        {
            var allBooksServiceModel = _booksService.GetAllBooks(currentPage);

            if (allBooksServiceModel.Books?.Count() == 0) 
                this.ModelState.AddModelError(String.Empty, NoBooksFound);

            return View(allBooksServiceModel);
        }

        public IActionResult Details([FromQuery] string bookId)
        {
            var userId = this.User.GetId();
            var bookDetailsServiceModel = _booksService.GetBookDetails
                (bookId, userId);

            if (bookDetailsServiceModel == null)
                this.ModelState.AddModelError(String.Empty, BookNotFound);

            return View(bookDetailsServiceModel);
        }

        [Authorize(Roles = UserRoleName)]
        public IActionResult AddBook()
        {
            var addBookFormModel = new AddBookFormModel()
                { Genres = _booksService.GetAllGenresServiceModels() };

            if (addBookFormModel.Genres?.Count() < 1) 
                this.ModelState.AddModelError(String.Empty, SomethingWentWrong);

            return View(addBookFormModel);
        }

        [Authorize(Roles = UserRoleName)]
        [HttpPost]
        public IActionResult AddBook(AddBookFormModel addBookFormModel)
        {
            var userId = this.User.GetId();
            var isBookAdded = _booksService
                .AddBookAndReturnBoolean(addBookFormModel, userId);

            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError(String.Empty, SomethingWentWrong);

                return View("AddBook");
            }

            if (!isBookAdded)
            {
                this.ModelState.AddModelError(String.Empty, TitleAlreadyExists);

                return View("AddBook");
            }

            return Redirect(nameof(this.MyLibrary));
        }

        [Authorize(Roles = UserRoleName)]
        public IActionResult MyLibrary([FromQuery] int currentPage)
        {
            var userId = User.GetId();
            var allBooksServiceModel = _booksService
                .GetMyLibrary(currentPage, userId);

            if (allBooksServiceModel.Books?.Count() == 0) 
                this.ModelState.AddModelError(String.Empty, NoBooksFound);

            return View(allBooksServiceModel);
        }

        
    }
}
