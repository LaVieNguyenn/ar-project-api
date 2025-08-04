using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class TransactionUpdateDto
    {
        public string UserId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Note { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Method { get; set; } = null!;
    }
}
