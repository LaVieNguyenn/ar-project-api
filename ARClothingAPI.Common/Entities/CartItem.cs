using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.Entities
{
    public class CartItem
    {
        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("model3DUrl")]
        public string Model3DUrl { get; set; }
    }
}
