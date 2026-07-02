using Clinc.Data.Static;
using Clinc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Clinc.Data
{
    public class AppDbInitializer
    {

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            // Roles
            if (!await roleManager.RoleExistsAsync(UserRole.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));

            if (!await roleManager.RoleExistsAsync(UserRole.User))
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));

            if (!await roleManager.RoleExistsAsync(UserRole.SuperAdmin))
                await roleManager.CreateAsync(new IdentityRole(UserRole.SuperAdmin));

            // ================= SUPER ADMIN =================
            string superAdminEmail = "Super@Super.com";

            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin == null)
            {
                var newSuperAdmin = new AppUser()
                {
                    FullName = "Mohamed Azzam",
                    Address = "Quwesna - El Menoufia",
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newSuperAdmin, "Mohamed@682001!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newSuperAdmin, UserRole.SuperAdmin);
                }
            }

            // ================= ADMIN =================
            string adminEmail = "Admin@Admin.com";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                var newAdmin = new AppUser()
                {
                    FullName = "Admin-1",
                    Address = "Quwesna - El Menoufia",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@0001!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, UserRole.Admin);
                }
            }

            // ================= USER =================
            string userEmail = "User@User.com";

            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                var newUser = new AppUser()
                {
                    FullName = "User-1",
                    Address = "Quwesna - El Menoufia",
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, "User@0001!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, UserRole.User);
                }
            }
        }
    }
}
