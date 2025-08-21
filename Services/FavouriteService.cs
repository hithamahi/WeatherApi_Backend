using MongoDB.Driver;
using WeatherApi.Model;
using Microsoft.AspNetCore.SignalR;
using WeatherApi.SignalR;

namespace WeatherApi.Services
{
    public class FavouriteService
    {
        private readonly IMongoCollection<UserFavourites> _userFavourites;
        private readonly IHubContext<FavouriteHub> _hubContext;

        public FavouriteService(MongoDbSettings settings, IHubContext<FavouriteHub> hubContext)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _userFavourites = database.GetCollection<UserFavourites>(settings.UsersCollection);
            _hubContext = hubContext;
        }

        public async Task<UserFavourites> GetByEmailAsync(string email)
        {
            return await _userFavourites.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task InsertUserFavouriteAsync(UserFavourites user)
        {
            await _userFavourites.InsertOneAsync(user);
            await _hubContext.Clients.All.SendAsync("FavouriteUpdated", user.Email);
        }

        public async Task ReplaceUserFavouriteAsync(string email, UserFavourites updated)
        {
            await _userFavourites.ReplaceOneAsync(u => u.Email == email, updated);
            await _hubContext.Clients.All.SendAsync("FavouriteUpdated", email);
        }

        public async Task RemoveFavouriteAsync(string email, string city)
        {
            var user = await GetByEmailAsync(email);

            if (user == null) return;

            bool removed = user.Favourites.RemoveAll(c => string.Equals(c, city, StringComparison.OrdinalIgnoreCase)) > 0;

            if (removed)
            {
                await ReplaceUserFavouriteAsync(email, user);
            }
        }
    }
}
