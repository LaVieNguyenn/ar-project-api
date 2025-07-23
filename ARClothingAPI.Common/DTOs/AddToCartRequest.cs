using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class AddToCartRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
