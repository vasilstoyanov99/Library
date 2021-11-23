namespace Library.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Global.GlobalConstants.DataValidations;

    public class Book
    {
        public Book() => Id = Guid.NewGuid().ToString();

        public string Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(AuthorNameMaxLength)]
        public string Author { get; set; }

        [Required]
        [MaxLength(ShortDescriptionMaxLength)]
        public string ShortDescription { get; set; }

        [Required]
        [MaxLength(LongDescriptionMaxLength)]
        public string LongDescription { get; set; }

        [Required]
        [RegularExpression(UrlRegex)]
        public string ImageUrl { get; set; }

        [Required]
        public string GenreId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
