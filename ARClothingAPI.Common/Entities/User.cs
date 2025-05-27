using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ARClothingAPI.Common.Entities
{
    public class User : BaseEntity
    {
        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("avatarUrl")]
        public string? AvatarUrl { get; set; }

        [BsonElement("gender")]
        public int Gender { get; set; }

        [BsonElement("roleId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RoleId { get; set; } = null!;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("height")]
        public double Height { get; set; }

        [BsonElement("weight")]
        public double Weight { get; set; }
    }
}
