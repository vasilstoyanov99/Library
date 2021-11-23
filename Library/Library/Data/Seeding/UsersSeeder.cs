namespace Library.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using static Areas.User.UserConstants;
    using static Data.UserSeedData;

    public class UsersSeeder : ISeeder
    {
        public void Seed(LibraryDbContext data, IServiceProvider serviceProvider)
        {
            var emails = new List<string>() { User1Email, User2Email};

            if (!data.Users.Any(u => u.Email == emails.First() && u.Email == emails.Last()))
            {
                var userManager =
                    serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                foreach (var email in emails)
                {
                    Task.
                        Run(async () =>
                        {
                            var user = new IdentityUser()
                                { Email = email, UserName = email};
                            await userManager.CreateAsync(user, Password);
                            await userManager.AddToRoleAsync(user, UserRoleName);
                        })
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }
    }
}
