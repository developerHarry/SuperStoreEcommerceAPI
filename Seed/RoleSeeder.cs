using Microsoft.AspNetCore.Identity;

namespace SuperStoreEcommerceAPI.Seed
{
    public class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "admin", "Customer", "StoreManager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
