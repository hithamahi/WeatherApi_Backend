using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherApi.Model
{
    [BsonIgnoreExtraElements]
    public class FavouriteCity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("CityName")]
        public string CityName { get; set; } = string.Empty;

        [BsonElement("addedOn")]
        public DateTime AddedOn { get; set; }
    }




    public class MongoDbSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string FavouritesCollection { get; set; }

        public required string UsersCollection { get; set; }

       
    }
}
