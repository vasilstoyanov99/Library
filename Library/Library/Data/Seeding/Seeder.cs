﻿namespace Library.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Seeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            var seeders = new List<ISeeder>()
            {
                new RolesSeeder(),
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(data, serviceProvider);
            }
        }
    }
}
