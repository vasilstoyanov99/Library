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
                new GenresSeeder(),
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(data, serviceProvider);
            }
        }
    }
}
