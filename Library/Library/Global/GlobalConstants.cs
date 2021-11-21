namespace Library.Global
{
    public static class GlobalConstants
    {
        public static class DataValidations
        {
            public const int GenreNameMaxLength = 60;

            public const int GenreNameMinLength = 3;

            public const int TitleMaxLength = 100;

            public const int TitleMinLength = 5;

            public const int AuthorNameMaxLength = 100;

            public const int AuthorNameMinLength = 3;

            public const int ShortDescriptionMaxLength = 300;

            public const int ShortDescriptionMinLength = 20;

            public const int LongDescriptionMaxLength = 1000;

            public const int LongDescriptionMinLength = 100;

            public const string UrlRegex = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";
        }

        public static class Paging
        {
            public const int CurrentPageStart = 1;

            public const int ThreeCardsPerPage = 3;
        }

        public static class ErrorMessages
        {
            public const string SomethingWentWrong = "Ups... something went wrong! ¯\\_(ツ)_/¯";

            public const string NoBooksFound = "No books found!";

            public const string BookNotFound = "The book was not found!";
        }
    }
}
