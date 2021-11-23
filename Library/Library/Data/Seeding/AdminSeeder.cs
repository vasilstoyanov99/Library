using System.Linq;

namespace Library.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using static Areas.Admin.AdminConstants;
    public class AdminSeeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            const string email = "admin@library.bg";

            if (!data.Users.Any(e => e.Email == email))
            {
                var userManager =
                    serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                Task
                    .Run(async () =>
                    {
                        var admin = new IdentityUser() { Email = email, UserName = email};
                        await userManager.CreateAsync(admin, "123456");
                        await userManager.AddToRoleAsync(admin, AdminRoleName);
                    })
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
