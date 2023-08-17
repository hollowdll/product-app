using Microsoft.Extensions.Options;
using ProductApi.Models;
using Microsoft.AspNetCore.Identity;
using ProductApi.Services;
using BCrypt.Net;

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
            int rounds = 12;

            var testUserRoles = new List<AppRole>();
            testUserRoles.Add(userRole);
            
            string hashedPassword1 = BCrypt.Net.BCrypt
                .EnhancedHashPassword("Password1!", HashType.SHA384, rounds);
            var testUser = new AppUser("TestUser", hashedPassword1, testUserRoles);
            users.Add(testUser);

            var adminUserRoles = new List<AppRole>();
            adminUserRoles.Add(userRole);
            adminUserRoles.Add(adminRole);

            string hashedPassword2 = BCrypt.Net.BCrypt
                .EnhancedHashPassword("Password2!", HashType.SHA384, rounds);
            var adminUser = new AppUser("AdminUser", hashedPassword2, adminUserRoles);
            users.Add(adminUser);

            await userService.AddManyUsersAsync(users);
        }
    }

    public static async void CreateInitialProducts(ProductsService productsService)
    {
        var products = await productsService.GetProductsAsync();
        if (!products.Any())
        {
            var product1 = new Product
            {
                Name = "Phone",
                Manufacturer = "Company A",
                Price = 250,
                Published = DateTime.Now
            };
            var product2 = new Product
            {
                Name = "Laptop",
                Manufacturer = "Company B",
                Price = 700,
                Published = DateTime.Now
            };
            var product3 = new Product
            {
                Name = "Toilet paper",
                Manufacturer = "Company C",
                Price = 99,
                Published = DateTime.Now
            };
            var product4 = new Product
            {
                Name = "Keyboard",
                Manufacturer = "Company D",
                Price = 50,
                Published = DateTime.Now
            };
            var product5 = new Product
            {
                Name = "Chair",
                Manufacturer = "Company E",
                Price = 65,
                Published = DateTime.Now
            };

            products.Add(product1);
            products.Add(product2);
            products.Add(product3);
            products.Add(product4);
            products.Add(product5);

            await productsService.AddManyProductsAsync(products);
        }
    }
}
