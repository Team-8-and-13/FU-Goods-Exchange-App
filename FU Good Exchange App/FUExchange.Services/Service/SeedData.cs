using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Repositories.Entity;

namespace FUExchange.Services.Service
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Ensure roles exist
            string[] roles = { ApplicationRole.Admin, ApplicationRole.UserPolicy, ApplicationRole.Moderator };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            // Ensure admin user exists
            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    UserInfo = new UserInfo { FullName = "Administrator" }
                };
                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin);
                }
            }
            // Ensure another admin user exists
            //var AdminUser2 = await userManager.FindByNameAsync("adminUser2");

            //if (AdminUser2 == null)
            //{
            //    AdminUser2 = new ApplicationUser
            //    {
            //        UserName = "anotherAdmin",
            //        Email = "anotherAdmin@example.com",
            //        UserInfo = new UserInfo { FullName = "Administrator2" }
            //    };
            //    var result = await userManager.CreateAsync(AdminUser2, "Admin2Password123!");
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(AdminUser2, ApplicationRole.Admin);
            //    }
            //}


            var moderatorUser = await userManager.FindByNameAsync("moderator");

            if (moderatorUser == null)
            {
                moderatorUser = new ApplicationUser
                {
                    UserName = "moderator",
                    Email = "moderator@example.com",
                    UserInfo = new UserInfo { FullName = "Moderator" }
                };
                var result = await userManager.CreateAsync(moderatorUser, "ModeratorPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(moderatorUser, ApplicationRole.Moderator);
                }
            }
        }
    }
}
