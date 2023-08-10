using Microsoft.Extensions.Options;
using ProductApi.Models;
using Microsoft.AspNetCore.Identity;
using ProductApi.Services;

namespace ProductApi.Data;

public static class SeedData
{
    public static async void CreateInitialRoles(RoleService roleService)
    {
        if (!roleService.GetRoles().Any())
        {
            var roles = new List<AppRole>();

            var userRole = new AppRole("User");
            roles.Add(userRole);

            var adminRole = new AppRole("Admin");
            roles.Add(adminRole);

            await roleService.AddManyRolesAsync(roles);
        }
    }

    public static async void CreateInitialUsers(RoleService roleService, UserService userService)
    {
        if (!userService.GetUsers().Any())
        {
            var users = new List<AppUser>();
            var userRole = await roleService.GetRoleByNameAsync("User");
            var adminRole = await roleService.GetRoleByNameAsync("Admin");

            var testUserRoles = new List<AppRole>();
            testUserRoles.Add(userRole);

            var testUser = new AppUser("TestUser", "Password10!", testUserRoles);
            users.Add(testUser);

            var adminUserRoles = new List<AppRole>();
            adminUserRoles.Add(userRole);
            adminUserRoles.Add(adminRole);

            var adminUser = new AppUser("AdminUser", "Password11!", adminUserRoles);
            users.Add(adminUser);

            await userService.AddManyUsersAsync(users);
        }
    }
}
