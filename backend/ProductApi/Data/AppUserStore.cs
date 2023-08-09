using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ProductApi.Models;
using Microsoft.Extensions.Options;

namespace ProductApi.Data;

public class AppUserStore : IUserStore<AppUser>
{
    private readonly IMongoCollection<AppUser> _usersCollection;

    public AppUserStore(
        IOptions<ProductDatabaseSettings> databaseSettings, ProductDbContext context)
    {
        _usersCollection = context.Database.GetCollection<AppUser>(
            databaseSettings.Value.UsersCollectionName);
    }

    public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
    {
        await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);

        return IdentityResult.Success;
    }
    
    public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
    {
        await _usersCollection.DeleteOneAsync(i => i.Id == user.Id);

        return IdentityResult.Success;
    }
    
    public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _usersCollection.Find(i => i.Id == userId).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<AppUser> FindByNameAsync(string normalizedUsername, CancellationToken cancellationToken)
    {
        return await _usersCollection.Find(i => i.NormalizedUsername == normalizedUsername).FirstOrDefaultAsync(cancellationToken);
    }
    
    public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUsername);
    }
    
    public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id);
    }
    
    public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Username);
    }
    
    public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUsername = normalizedName;

        return Task.CompletedTask;
    }
    
    public Task SetUserNameAsync(AppUser user, string username, CancellationToken cancellationToken)
    {
        user.Username = username;

        return Task.CompletedTask;
    }
    
    public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
    {
        await _usersCollection.ReplaceOneAsync(i => i.Id == user.Id, user);

        return IdentityResult.Success;
    }

    public void Dispose()
    {
    }
}
