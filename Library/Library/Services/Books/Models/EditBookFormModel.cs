namespace Library.Services.Books.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EditBookFormModel : AddBookFormModel
    {
        [Required] 
        public string Id { get; set; }

        public string CurrentGenre { get; set; }
    }
}
