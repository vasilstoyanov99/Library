namespace Library.Services.Books.Models
{
    using System.Collections.Generic;

    public class AddBookFormModel : EditBookFormModel
    {
        public string GenreId { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }
    }
}
