using ProductApi.Models;

namespace ProductApi.Dtos;

public class AppUserDto
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public IList<AppRoleDto> Roles { get; set; } = null!;
}