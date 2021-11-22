using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using static Areas.Admin.AdminConstants;
    using static Areas.User.UserConstants;

    public class RolesSeeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                        return;

                    var adminRole = new IdentityRole { Name = AdminRoleName };

                    await roleManager.CreateAsync(adminRole);

                    if (await roleManager.RoleExistsAsync(UserRoleName))
                        return;

                    var userRole = new IdentityRole { Name = UserRoleName };

                    await roleManager.CreateAsync(userRole);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
