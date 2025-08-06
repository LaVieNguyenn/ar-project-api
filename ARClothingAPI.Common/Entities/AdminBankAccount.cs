using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.Entities
{
    public class AdminBankAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        [BsonElement("accountNumber")]
        public string AccountNumber { get; set; } = null!;
        [BsonElement("bankCode")]
        public string BankCode { get; set; } = null!;
        [BsonElement("accountName")]
        public string AccountName { get; set; } = null!;
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }

}
