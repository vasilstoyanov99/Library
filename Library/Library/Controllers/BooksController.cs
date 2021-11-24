using System.Collections.Generic;
using Library.Infrastructure;
using Library.Services.Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Controllers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Services.Books;

    using static Global.GlobalConstants.ErrorMessages;
    using static Global.GlobalConstants.MemoryCacheKeys;
    using static Global.GlobalConstants.Notifications;
    using static Global.CustomRoles;

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
            var allBooksServiceModel = _booksService.GetAllBooks(currentPage);

            if (allBooksServiceModel.Books?.Count() == 0) 
                ModelState.AddModelError(String.Empty, NoBooksFound);

            return View(allBooksServiceModel);
        }

        public IActionResult Details([FromQuery]  string bookId)
        {
            var userId = User.GetId();
            var bookDetailsServiceModel = _booksService.GetBookDetails
                (bookId, userId);

            if (bookDetailsServiceModel == null)
                ModelState.AddModelError(String.Empty, BookNotFound);

            return View(bookDetailsServiceModel);
        }

        [Authorize(Roles = AdminOrUser)]
        public IActionResult AddBook()
        {
            var addBookFormModel = new AddBookFormModel()
                { Genres = _booksService.GetAllGenresServiceModels() };

            if (addBookFormModel.Genres?.Count() < 1) 
                ModelState.AddModelError(String.Empty, SomethingWentWrong);

            return View(addBookFormModel);
        }

        [HttpPost]
        [Authorize(Roles = AdminOrUser)]
        public IActionResult AddBook(AddBookFormModel addBookFormModel)
        {
            var genres = GetGenresAndSetCacheIfNeeded();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, SomethingWentWrong);
                addBookFormModel.Genres = genres;

                return View("AddBook", addBookFormModel);
            }

            var userId = User.GetId();
            var results = _booksService
                .AddBookAndReturnBooleans(addBookFormModel, userId);

            if (results.doesTitleExistsInDb)
                ModelState.AddModelError(String.Empty, TitleAlreadyExists);

            if (results.genreDoesNotExistsInDb) 
                ModelState.AddModelError(String.Empty, GenreDoesNotExists);

            if (ModelState.ErrorCount > 0)
            {
                addBookFormModel.Genres = genres;
                return View("AddBook", addBookFormModel);
            }

            TempData[SuccessfullyAddedBookKey] = SuccessfullyAddedBook;

            return RedirectToAction(nameof(MyLibrary));
        }

        [Authorize(Roles = AdminOrUser)]
        public IActionResult EditBook(string bookId)
        {
            var editBookFormModel = _booksService.GetEditBookFormModel(bookId);

            if (editBookFormModel == null) 
                ModelState.AddModelError(String.Empty, BookNotFound);

            return View(editBookFormModel);
        }

        [HttpPost]
        [Authorize(Roles = AdminOrUser)]
        public IActionResult Edit(EditBookFormModel editBookFormModel)
        {
            var genres = GetGenresAndSetCacheIfNeeded();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, SomethingWentWrong);
                editBookFormModel.Genres = genres;

                return View("EditBook", editBookFormModel);
            }

            var results = _booksService.EditBookAndReturnBooleans(editBookFormModel);

            if (results.bookDoesNotExistsInDb) 
                ModelState.AddModelError(String.Empty, BookNotFound);

            if (results.genreDoesNotExistsInDb) 
                ModelState.AddModelError(String.Empty, GenreDoesNotExists);

            if (ModelState.ErrorCount > 0)
            {
                editBookFormModel.Genres = genres;
                return View("EditBook", editBookFormModel);
            }

            TempData[SuccessfullyEditedBookKey] = SuccessfullyEditedBook;

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = AdminOrUser)]
        public IActionResult Delete([FromQuery] string bookId)
        {
            var isBookDeleted = _booksService.DeleteBookAndReturnBoolean(bookId);

            if (!isBookDeleted)
            {
                TempData[UnsuccessfullyDeletedBookKey] = UnsuccessfullyDeletedBook;

                return RedirectToAction(nameof(All));
            }

            TempData[SuccessfullyDeletedBookKey] = SuccessfullyDeletedBook;

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = AdminOrUser)]
        public IActionResult MyLibrary([FromQuery] int currentPage)
        {
            var userId = User.GetId();
            var allBooksServiceModel = _booksService
                .GetMyLibrary(currentPage, userId);

            if (allBooksServiceModel.Books?.Count() == 0) 
                ModelState.AddModelError(String.Empty, NoBooksFound);

            return View(allBooksServiceModel);
        }

        private List<GenreServiceModel> GetGenresAndSetCacheIfNeeded()
        {
            var genres = _cache.Get<List<GenreServiceModel>>(GenresCacheKey);

            if (genres == null)
            {
                genres = _booksService.GetAllGenresServiceModels();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                _cache.Set(GenresCacheKey, genres, cacheOptions);
            }

            return genres;
        }
    }
}
