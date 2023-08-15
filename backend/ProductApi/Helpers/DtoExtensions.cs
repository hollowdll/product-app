using ProductApi.Models;
using ProductApi.Dtos;

namespace ProductApi.Helpers;

public static class DtoExtensions
{
    /// <summary>
    /// Converts <see cref="Product" /> class to <see cref="ProductDto" /> class.
    /// </summary>
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Manufacturer = product.Manufacturer,
            Price = product.Price,
            Published = product.Published
        };
    }

    /// <summary>
    /// Converts <see cref="AppUser" /> class to <see cref="AppUserDto" /> class.
    /// </summary>
    public static AppUserDto ToDto(this AppUser appUser)
    {
        return new AppUserDto
        {
            Id = appUser.Id,
            Username = appUser.Username,
            Roles = appUser.Roles
                .Select(i => i.ToDto())
                .ToList()
        };
    }

    /// <summary>
    /// Converts <see cref="AppRole" /> class to <see cref="AppRoleDto" /> class.
    /// </summary>
    public static AppRoleDto ToDto(this AppRole appRole)
    {
        return new AppRoleDto
        {
            Name = appRole.Name
        };
    }
}
