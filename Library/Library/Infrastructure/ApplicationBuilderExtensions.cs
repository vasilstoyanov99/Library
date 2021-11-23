namespace Library.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;

    using Data.Seeding;
    using Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;
            var data = scopedServices.ServiceProvider
                .GetService<LibraryDbContext>();

            data.Database.Migrate();

            var seeder = new Seeder();
            seeder.Seed(data, services);

            return app;
        }
    }
}