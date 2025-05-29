using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ARClothingAPI.Common.Entities
{
    public class Category : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("parentCategoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentCategoryId { get; set; }
    }
}
