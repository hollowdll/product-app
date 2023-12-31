using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductApi.Models;

public class AppUser
{
    public AppUser() {}

    public AppUser(string username, string password, List<AppRole> roles)
    {
        Username = username;
        NormalizedUsername = username.ToUpper();
        Password = password;
        Roles = roles;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string NormalizedUsername { get; set; } = null!;

    public string Password { get; set; } = null!;

    public IList<AppRole> Roles { get; set; } = null!;
}
