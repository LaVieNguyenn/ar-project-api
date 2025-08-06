using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ARClothingAPI.Common.Entities
{
    public class Transaction : BaseEntity
    {
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("avatar")]
        public string Avatar { get; set; } = "";

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = null!; // "plan_purchase"

        [BsonElement("method")]
        public string Method { get; set; } = null!; // "bank_transfer"

        [BsonElement("note")]
        public string Note { get; set; } = "";

        [BsonElement("planId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? PlanId { get; set; }

        // --- Banking/QR Tracking ---
        [BsonElement("paymentContent")]
        public string PaymentContent { get; set; } = null!; // Nội dung chuyển khoản - tracking

        [BsonElement("bankName")]
        public string? BankName { get; set; }               // Ngân hàng nhận/chuyển

        [BsonElement("bankRefNumber")]
        public string? BankRefNumber { get; set; }          // Số tham chiếu giao dịch ngân hàng

        [BsonElement("status")]
        public string Status { get; set; } = "approved";    // "approved", (có thể bổ sung "pending", "rejected" nếu muốn mở rộng)

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; } = "system";

        [BsonElement("updatedBy")]
        public string UpdatedBy { get; set; } = "system";
    }
}
