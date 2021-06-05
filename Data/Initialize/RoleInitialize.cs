using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestForGraduates.Models;

namespace TestForGraduates.Data.Initialize
{
    public class RoleInitialize
    {
        // инициализируем две роли (admin, teacher)
        public static async Task InitialRole(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            string adminEmail = "emailAdmin@mail.ru";
            string password = "passwordAdmin";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("teacher") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("teacher"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
