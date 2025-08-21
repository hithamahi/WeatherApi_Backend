using MongoDB.Driver;
using WeatherApi.Model;

namespace WeatherApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        
        
        
        public UserService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollection);
            
        }

        public async Task AddUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task<List<User>> GetUsersAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User> GetUserByUsername(string username) =>
            await _users.Find(u => u.Username == username).FirstOrDefaultAsync();

        

    }
}
