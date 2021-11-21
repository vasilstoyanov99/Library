namespace Library.Data.Seeding
{
    using System;

    public interface ISeeder
    {
        void Seed(LibraryDbContext data, IServiceProvider serviceProvider);
    }
}
