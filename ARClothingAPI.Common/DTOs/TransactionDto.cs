using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class TransactionDto
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Avatar { get; set; } = "";
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string Method { get; set; } = null!;
        public string Note { get; set; } = "";
        public string? PlanId { get; set; }
        public string PaymentContent { get; set; } = null!;
        public string? BankName { get; set; }
        public string? BankRefNumber { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
    }

}
