namespace Library.Services.Books.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Global.GlobalConstants.DataValidations;
    using static Global.GlobalConstants.ErrorMessages;

    public class AddBookFormModel
    {
        [Required]
        [StringLength(TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = TitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(AuthorNameMaxLength,
            MinimumLength = AuthorNameMinLength,
            ErrorMessage = AuthorNameLength)]
        public string Author { get; set; }

        [Required]
        [StringLength(ShortDescriptionMaxLength,
            MinimumLength = ShortDescriptionMinLength,
            ErrorMessage = ShortDescriptionLength)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Required]
        [StringLength(LongDescriptionMaxLength,
            MinimumLength = LongDescriptionMinLength,
            ErrorMessage = LongDescriptionLength)]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [Required]
        [RegularExpression(UrlRegex)]
        [Display(Name = "Book cover image URL")]
        public string ImageUrl { get; set; }

        [Required]
        public string GenreId { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }
    }
}
