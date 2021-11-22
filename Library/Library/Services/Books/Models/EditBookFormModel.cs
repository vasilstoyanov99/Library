using System.ComponentModel.DataAnnotations;

namespace Library.Services.Books.Models
{
    public class EditBookFormModel : AddBookFormModel
    {
        [Required] 
        public string Id { get; set; }

        public string CurrentGenre { get; set; }
    }
}
