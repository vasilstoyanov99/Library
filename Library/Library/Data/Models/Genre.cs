namespace Library.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using static Global.GlobalConstants.DataValidations;

    public class Genre
    {
        public Genre()
        {
            Id = Guid.NewGuid().ToString();
            Books = new HashSet<Book>();
        }

        public string Id { get; init; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
