using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ARClothingAPI.Common.Entities
{
    public class Cart
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; } // hoặc ObjectId nếu user

        [BsonElement("items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
