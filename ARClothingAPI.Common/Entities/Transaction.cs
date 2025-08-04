using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ARClothingAPI.Common.Entities
{
    public class Transaction : BaseEntity
    {
        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = "topup";

        [BsonElement("method")]
        public string Method { get; set; } = "manual";

        [BsonElement("note")]
        public string Note { get; set; } = null!;

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; } = null!;

        [BsonElement("updatedBy")]
        public string UpdatedBy { get; set; } = null!;
    }
}