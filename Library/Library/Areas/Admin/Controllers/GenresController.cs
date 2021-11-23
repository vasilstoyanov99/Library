namespace Library.Areas.Admin.Controllers
{
    using System;
    using Services.Genres;
    using Library.Areas.Admin.Services.Genres.Models;
    using Microsoft.AspNetCore.Mvc;
    using static Global.GlobalConstants.ErrorMessages;
    using static Global.GlobalConstants.Notifications;

    public class GenresController : AdminController
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService) => _genresService = genresService;

        public IActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddGenreFormModel addGenreFormModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, SomethingWentWrong);
                return RedirectToAction(nameof(this.AddGenre));
            }

            var isGenreAdded = _genresService.AddGenreAndReturnBoolean(addGenreFormModel.Name);

            if (!isGenreAdded)
            {
                ModelState.AddModelError(String.Empty, GenreNameAlreadyExists);
                return RedirectToAction(nameof(this.AddGenre));
            }

            this.TempData[SuccessfullyAddedGenreKey] = SuccessfullyAddedGenre;

            return RedirectToAction(nameof(this.AddGenre));
        }
    }
}
