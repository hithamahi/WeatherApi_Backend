using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherApi.Model
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; } = "";

        [BsonElement("email")]
        public string Email { get; set; } = "";

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = "";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [BsonElement("favouriteCities")]
        public List<string> FavouriteCities { get; set; } = new();
    }
}
