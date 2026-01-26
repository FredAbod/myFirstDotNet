using MongoDB.Driver;
using test_dotnet.Models;

namespace test_dotnet.Services;

public class UserService
{
  private readonly IMongoCollection<User> _users;

  public UserService(IConfiguration config)
  {
    var client = new MongoClient(config["MongoDB:ConnectionString"]);
    var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
    _users = database.GetCollection<User>("users");
  }

  public async Task<List<User>> GetAllAsync()
  {
    return await _users.Find(_=> true).ToListAsync();
  }

  public async Task<User?> GetEmailAsync(string email)
  {
    return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
  }

public async Task<User?> GetByIdAsync(int id)
  {
    return await _users.Find(u => u.Id == id.ToString()).FirstOrDefaultAsync();
  }
  
  public async Task<User> CreateAsync(User user)
  {
    await _users.InsertOneAsync(user);
    return user;
  }

  public async Task UpdateAsync(string id, User user)
  {
    await _users.ReplaceOneAsync(u => u.Id == id, user);
  }

  public async Task DeleteAsync(string id)
  {
    await _users.DeleteOneAsync(u => u.Id == id);
  }
}
