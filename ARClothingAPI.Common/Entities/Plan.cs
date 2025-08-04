using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ARClothingAPI.Common.Entities
{
    public class Plan : BaseEntity
    {
        [BsonElement("name")] public string Name { get; set; } = null!;
        [BsonElement("description")] public string Description { get; set; } = null!;
        [BsonElement("durationMonths")] public int DurationMonths { get; set; }
        [BsonElement("price")] public decimal Price { get; set; }
        [BsonElement("discountPercent")] public decimal DiscountPercent { get; set; }
        [BsonElement("isActive")] public bool IsActive { get; set; } = true;
        [BsonElement("createdBy")] public string CreatedBy { get; set; } = null!;
        [BsonElement("updatedBy")] public string UpdatedBy { get; set; } = null!;
    }
}
