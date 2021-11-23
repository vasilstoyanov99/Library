namespace Library.Areas.Admin.Services.Genres
{
    using System.Linq;

    using Ganss.XSS;

    using Library.Data.Models;
    using Data;

    public class GenresService : IGenresService
    {
        private readonly LibraryDbContext _data;

        public GenresService(LibraryDbContext data) => _data = data;

        public bool AddGenreAndReturnBoolean(string name)
        {
            var htmlSanitizer = new HtmlSanitizer();
            name = htmlSanitizer.Sanitize(name).Trim();

            if (_data.Genres.Any(g => g.Name == name))
                return false;
            
            var newGenre = new Genre() { Name = name };
            _data.Genres.Add(newGenre);
            _data.SaveChanges();

            return true;
        }
    }
}
