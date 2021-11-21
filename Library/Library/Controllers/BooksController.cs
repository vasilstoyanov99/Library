using System.Linq.Expressions;

namespace Library.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Library.Services.Books;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Global.GlobalConstants.ErrorMessages;

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

        public IActionResult Details()
        {
            
            return null;
        }
    }
}
