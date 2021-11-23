namespace Library.Areas.Admin.Services.Genres.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Global.GlobalConstants.DataValidations;
    using static Global.GlobalConstants.ErrorMessages;
    public class AddGenreFormModel
    {
        [Required]
        [StringLength(GenreNameMaxLength,
            MinimumLength = GenreNameMinLength,
            ErrorMessage = GenreNameLength)]
        public string Name { get; set; }
    }
}
