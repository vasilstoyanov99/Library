namespace Library.Data.Seeding
{
    using System;
    using System.Collections.Generic;

    public class Seeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            var seeders = new List<ISeeder>()
            {
                new RolesSeeder(),
                new UsersSeeder(),
                new AdminSeeder(),
                new GenresSeeder(),
                new BooksSeeder()
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(data, serviceProvider);
            }
        }
    }
}
