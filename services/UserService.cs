using MongoDB.Driver;
using test_dotnet.Models;

namespace test_dotnet.Services;

/// <summary>
/// Service for user database operations
/// Contains all the business logic for users
/// </summary>
public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _users = database.GetCollection<User>("users");
    }

    /// <summary>
    /// Get all users from database
    /// </summary>
    public async Task<List<User>> GetAllAsync()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Find user by email
    /// </summary>
    public async Task<User?> GetEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Find user by ID
    /// </summary>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _users.Find(u => u.Id == id.ToString()).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Create new user (password is hashed here)
    /// </summary>
    public async Task<User> CreateAsync(User user)
    {
        // Hash password before saving - NEVER store plain text!
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await _users.InsertOneAsync(user);
        return user;
    }

    /// <summary>
    /// Update existing user
    /// </summary>
    public async Task UpdateAsync(string id, User user)
    {
        await _users.ReplaceOneAsync(u => u.Id == id, user);
    }

    /// <summary>
    /// Delete user by ID
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        await _users.DeleteOneAsync(u => u.Id == id);
    }
}
