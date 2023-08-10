using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ProductApi.Models;
using Microsoft.Extensions.Options;

namespace ProductApi.Data;

public class AppRoleStore : IQueryableRoleStore<AppRole>
{
    private readonly IMongoCollection<AppRole> _rolesCollection;

    public AppRoleStore(
        IOptions<ProductDatabaseSettings> databaseSettings, ProductDbContext context)
    {
        _rolesCollection = context.Database.GetCollection<AppRole>(
            databaseSettings.Value.RolesCollectionName);
    }

    public IQueryable<AppRole> Roles => _rolesCollection.AsQueryable();

    public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
    {
        await _rolesCollection.InsertOneAsync(role, cancellationToken: cancellationToken);

        return IdentityResult.Success;
    }
    
    public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
    {
        await _rolesCollection.DeleteOneAsync(i => i.Id == role.Id, cancellationToken);

        return IdentityResult.Success;
    }
    
    public async Task<AppRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        return await _rolesCollection.Find(i => i.Id == roleId).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<AppRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        return await _rolesCollection.Find(i => i.NormalizedName == normalizedRoleName).FirstOrDefaultAsync(cancellationToken);
    }
    
    public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }
    
    public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id);
    }
    
    public Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }
    
    public Task SetNormalizedRoleNameAsync(AppRole role, string normalizedName, CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;

        return Task.CompletedTask;
    }
    
    public Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;

        return Task.CompletedTask;
    }
    
    public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
    {
        await _rolesCollection.ReplaceOneAsync(i => i.Id == role.Id, role, cancellationToken: cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<List<AppRole>> FindAllAsync(CancellationToken cancellationToken)
    {
        return await _rolesCollection.Find(_ => true).ToListAsync(cancellationToken);
    }

    

    public void Dispose()
    {
    }
}
