using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Links.Models
{
    public class Link
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string AbbreviatedTitle { get; set; }
        public string Hash { get; set; }
        public int NumberOfTransitions { get; set; }
    }
}
