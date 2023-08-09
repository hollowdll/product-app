using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductApi.Models;

public class AppUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public IList<AppRole> Roles { get; } = null!;
}
