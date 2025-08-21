using Microsoft.AspNetCore.Identity;
using SuperStoreEcommerceAPI.Models;

namespace SuperStoreEcommerceAPI.Seed
{
    public class AdminSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "harrykachigamba11@outlook.com";
            var existing = await userManager.FindByNameAsync(email);
            if (existing != null) return;

            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                FullName = "System Admin"
            };

            var result = await userManager.CreateAsync(user, "Admin#12345");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
