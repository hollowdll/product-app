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

            var userRole = new AppRole()
            {
                Name = "User",
                NormalizedName = "USER"
            };
            roles.Add(userRole);

            var adminRole = new AppRole()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
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
            var testUser = new AppUser()
            {
                Username = "TestUser1",
                NormalizedUsername = "TESTUSER1",
                Password = "Password1!",
                Roles = testUserRoles,
            };
            users.Add(testUser);

            var adminUserRoles = new List<AppRole>();
            adminUserRoles.Add(userRole);
            adminUserRoles.Add(adminRole);
            var adminUser = new AppUser()
            {
                Username = "AdminUser",
                NormalizedUsername = "ADMINUSER",
                Password = "Password2!",
                Roles = adminUserRoles,
            };
            users.Add(adminUser);

            await userService.AddManyUsersAsync(users);
        }
    }
}
