using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class TransactionCreateDto
    {
        public string UserId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Note { get; set; } = null!;
        public string Type { get; set; } = null!;            // Luôn "plan_purchase"
        public string Method { get; set; } = null!;          // "bank_transfer"
        public string? PlanId { get; set; }                  // Gói plan liên quan
        public string PaymentContent { get; set; } = null!;  // Nội dung chuyển khoản admin nhập
        public string? BankName { get; set; }                // Tên ngân hàng nhận
        public string? BankRefNumber { get; set; }           // Số tham chiếu ngân hàng
    }

}
