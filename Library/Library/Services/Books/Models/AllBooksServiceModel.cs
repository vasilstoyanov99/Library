namespace Library.Services.Books.Models
{
    using System.Collections.Generic;

    using static Global.GlobalConstants.Paging;

    public class AllBooksServiceModel
    {
        public AllBooksServiceModel() => CurrentPage = CurrentPageStart;

        public int CurrentPage { get; set; }

        public double MaxPage { get; set; }

        public IEnumerable<BookListingServiceModel> Books { get; set; }
    }
}
