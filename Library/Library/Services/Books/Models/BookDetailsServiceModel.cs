namespace Library.Services.Books.Models
{
    public class BookDetailsServiceModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string LongDescription { get; set; }

        public string ImageUrl { get; set; }

        public bool CanUserEdit { get; set; }
    }
}
