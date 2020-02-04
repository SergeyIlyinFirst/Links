using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Links.Models
{
    public class CounterOfVisits
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Counter { get; set; }
    }
}
