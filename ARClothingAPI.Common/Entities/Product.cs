using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ARClothingAPI.Common.Entities
{
    public class Product : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("images")]
        public List<string> Images { get; set; } = new List<string>();

        [BsonElement("model3DUrl")]
        public string? Model3DUrl { get; set; }

        [BsonElement("sizes")]
        public List<string> Sizes { get; set; } = new List<string>();

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = null!;

        [BsonElement("createdBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; } = null!;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }
}
