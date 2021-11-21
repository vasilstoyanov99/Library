using System.Collections.Generic;

namespace Library.Services.Books.Models
{
    using static Global.GlobalConstants.Paging;

    public class AllBooksServiceModel
    {
        public AllBooksServiceModel() => this.CurrentPage = CurrentPageStart;

        public int CurrentPage { get; set; }

        public double MaxPage { get; set; }

        public IEnumerable<BookListingServiceModel> Books { get; set; }
    }
}
