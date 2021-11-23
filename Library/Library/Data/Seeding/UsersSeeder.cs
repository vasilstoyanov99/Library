namespace Library.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using static Areas.User.UserConstants;

    public class UsersSeeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            var emails = new List<string>() { "user1@library.bg", "user2@library.bg" };

            if (!data.Users.Any(u => u.Email == emails.First() && u.Email == emails.Last()))
            {
                var userManager =
                    serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                const string password = "123456";

                foreach (var email in emails)
                {
                    Task.
                        Run(async () =>
                        {
                            var user = new IdentityUser()
                                { Email = email, UserName = email};
                            await userManager.CreateAsync(user, password);
                            await userManager.AddToRoleAsync(user, UserRoleName);
                        })
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }
    }
}
