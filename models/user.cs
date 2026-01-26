using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace test_dotnet.Models;


public class User
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Id { get; set;}

  [BsonElement("name")]
  public string Name {get; set; }= null;


  [BsonElement("email")]
  public string Email {get; set; }= null;

  [BsonElement("Password")]
  public string Password { get; set;}= null;

  [BsonElement("createdAt")]
  public DateTime createdAt { get; set; } = DateTime.UtcNow;
}
