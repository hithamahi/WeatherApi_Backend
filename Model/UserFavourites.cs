using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WeatherApi.Model
{
    public class UserFavourites
    {

        [BsonId]
        public ObjectId Id { get; set; }

       
        public string Email { get; set; }

       
        public List<string> Favourites { get; set; }
    }
}
