using Microsoft.AspNetCore.Identity;
using ProductApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductApi.Data;

namespace ProductApi.Services;

public class UserService : IDisposable
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMongoCollection<AppUser> _usersCollection;

    public UserService(
        IOptions<ProductDatabaseSettings> databaseSettings,
        ProductDbContext context,
        UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _usersCollection = context.Database.GetCollection<AppUser>(
            databaseSettings.Value.UsersCollectionName);
    }

    public IQueryable<AppUser> GetUsers()
    {
        return _userManager.Users;
    }

    public async Task<AppUser> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<AppUser> GetUserByUsernameCaseSensitiveAsync(string username)
    {
        return await _usersCollection.Find(i => i.Username == username).FirstOrDefaultAsync();
    }

    public async Task AddUserAsync(AppUser user)
    {
        // await _userManager.CreateAsync(user);
        await _usersCollection.InsertOneAsync(user);
    }

    public async Task AddManyUsersAsync(List<AppUser> users)
    {
        await _usersCollection.InsertManyAsync(users);
    }

    public void Dispose()
    {
    }
}
