using System.Linq.Expressions;
using Library.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Library.Services.Books;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
            {
                this.ModelState.AddModelError(String.Empty, NoBooksFound);
            }
            
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
    }
}
