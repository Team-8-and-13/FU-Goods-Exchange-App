using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using FUExchange.Contract.Repositories.Entity;
using System;
using System.Threading.Tasks;
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
            string[] roles = { ApplicationRole.Admin, ApplicationRole.UserPolicy,ApplicationRole.Moderator };
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
        }
    }
}
