using System.Collections.Generic;
using Library.Data.Models;
using Library.Infrastructure;
using Library.Services.Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Library.Services.Books;
    using System.Linq;
    using static Global.GlobalConstants.ErrorMessages;
    using static Global.GlobalConstants.MemoryCacheKeys;
    using static Global.GlobalConstants.SuccessNotifications;
    using static Global.CustomRoles;
    using static Areas.User.UserConstants;

    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly IMemoryCache _cache;

        public BooksController(IBooksService booksService, IMemoryCache cache)
        {
            _booksService = booksService;
            _cache = cache;
        }

        public IActionResult All([FromQuery] int currentPage)
        {
            var allBooksServiceModel = this._booksService.GetAllBooks(currentPage);

            if (allBooksServiceModel.Books?.Count() == 0) 
                this.ModelState.AddModelError(String.Empty, NoBooksFound);

            return View(allBooksServiceModel);
        }

        public IActionResult Details([FromQuery]  string bookId)
        {
            var userId = this.User.GetId();
            var bookDetailsServiceModel = this._booksService.GetBookDetails
                (bookId, userId);

            if (bookDetailsServiceModel == null)
                this.ModelState.AddModelError(String.Empty, BookNotFound);

            return View(bookDetailsServiceModel);
        }

        [Authorize(Roles = UserRoleName)]
        public IActionResult AddBook()
        {
            var addBookFormModel = new AddBookFormModel()
                { Genres = this._booksService.GetAllGenresServiceModels() };

            if (addBookFormModel.Genres?.Count() < 1) 
                this.ModelState.AddModelError(String.Empty, SomethingWentWrong);

            return View(addBookFormModel);
        }

        [HttpPost]
        [Authorize(Roles = UserRoleName)]
        public IActionResult AddBook(AddBookFormModel addBookFormModel)
        {
            var userId = this.User.GetId();
            var results = this._booksService
                .AddBookAndReturnBooleans(addBookFormModel, userId);
            var genres = this.GetGenresAndSetCacheIfNeeded();

            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError(String.Empty, SomethingWentWrong);
                addBookFormModel.Genres = genres;

                return View("AddBook", addBookFormModel);
            }

            if (results.doesTitleExistsInDb)
                this.ModelState.AddModelError(String.Empty, TitleAlreadyExists);

            if (results.genreDoesNotExistsInDb) 
                this.ModelState.AddModelError(String.Empty, GenreDoesNotExists);

            if (this.ModelState.ErrorCount > 0)
            {
                addBookFormModel.Genres = genres;
                return View("AddBook", addBookFormModel);
            }

            this.TempData[SuccessfullyAddedBookKey] = SuccessfullyAddedBook;

            return Redirect(nameof(this.MyLibrary));
        }

        [Authorize(Roles = AdminOrUser)]
        public IActionResult EditBook(string bookId)
        {
            var editBookFormModel = this._booksService.GetEditBookFormModel(bookId);

            if (editBookFormModel == null) 
                this.ModelState.AddModelError(String.Empty, BookNotFound);

            return View(editBookFormModel);
        }

        [HttpPost]
        [Authorize(Roles = AdminOrUser)]
        public IActionResult Edit(EditBookFormModel editBookFormModel)
        {
            var results = this._booksService.EditBookAndReturnBooleans(editBookFormModel);
            var genres = this.GetGenresAndSetCacheIfNeeded();

            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError(String.Empty, SomethingWentWrong);
                editBookFormModel.Genres = genres;

                return View("EditBook", editBookFormModel);
            }

            if (results.bookDoesNotExistsInDb) 
                this.ModelState.AddModelError(String.Empty, BookNotFound);

            if (results.genreDoesNotExistsInDb) 
                this.ModelState.AddModelError(String.Empty, GenreDoesNotExists);

            if (this.ModelState.ErrorCount > 0)
            {
                editBookFormModel.Genres = genres;
                return View("EditBook", editBookFormModel);
            }

            this.TempData[SuccessfullyEditedBookKey] = SuccessfullyEditedBook;

            return Redirect(nameof(this.MyLibrary));
        }

        [Authorize(Roles = UserRoleName)]
        public IActionResult MyLibrary([FromQuery] int currentPage)
        {
            var userId = User.GetId();
            var allBooksServiceModel = this._booksService
                .GetMyLibrary(currentPage, userId);

            if (allBooksServiceModel.Books?.Count() == 0) 
                this.ModelState.AddModelError(String.Empty, NoBooksFound);

            return View(allBooksServiceModel);
        }

        private List<GenreServiceModel> GetGenresAndSetCacheIfNeeded()
        {
            var genres = this._cache.Get<List<GenreServiceModel>>(GenresCacheKey);

            if (genres == null)
            {
                genres = _booksService.GetAllGenresServiceModels();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                this._cache.Set(GenresCacheKey, genres, cacheOptions);
            }

            return genres;
        }
    }
}
