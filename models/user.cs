using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace test_dotnet.Models;

/// <summary>
/// User entity - represents a user in MongoDB
/// This is the DATABASE model, not for API responses!
/// </summary>
[BsonIgnoreExtraElements]  // Ignore any extra fields in MongoDB that aren't in this class
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("Password")]  // Capital P to match existing MongoDB data
    public string Password { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
