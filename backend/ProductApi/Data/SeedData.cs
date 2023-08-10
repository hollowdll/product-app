using Microsoft.Extensions.Options;
using ProductApi.Models;
using Microsoft.AspNetCore.Identity;
using ProductApi.Services;

namespace ProductApi.Data;

public static class SeedData
{
    public static async void CreateInitialRoles(IServiceProvider serviceProvider)
    {
        using (var roleService = serviceProvider.GetRequiredService<RoleService>())
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
    }
}
