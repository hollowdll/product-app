using Microsoft.AspNetCore.Identity;
using ProductApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductApi.Data;

namespace ProductApi.Services;

public class RoleService : IDisposable
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMongoCollection<AppRole> _rolesCollection;

    public RoleService(
        IOptions<ProductDatabaseSettings> databaseSettings,
        ProductDbContext context,
        RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
        _rolesCollection = context.Database.GetCollection<AppRole>(
            databaseSettings.Value.RolesCollectionName);
    }

    public IQueryable<AppRole> GetRoles()
    {
        return _roleManager.Roles;
    }

    public async Task AddRoleAsync(AppRole role)
    {
        await _roleManager.CreateAsync(role);
    }

    public async Task AddManyRolesAsync(List<AppRole> roles)
    {
        await _rolesCollection.InsertManyAsync(roles);
    }

    public void Dispose()
    {
    }
}
