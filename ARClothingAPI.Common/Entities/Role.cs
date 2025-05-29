using MongoDB.Bson.Serialization.Attributes;

namespace ARClothingAPI.Common.Entities
{
    public class Role : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
