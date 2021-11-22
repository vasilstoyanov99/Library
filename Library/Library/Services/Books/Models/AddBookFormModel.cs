namespace Library.Services.Books.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddBookFormModel : EditBookFormModel
    {
        [Required]
        public string GenreId { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }
    }
}
