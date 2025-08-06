using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class EnrollPlanRequestDto
    {
        public string UserId { get; set; } = null!;
        public string PlanId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string PaymentContent { get; set; } = null!;
        public string? BankName { get; set; }
        public string? BankRefNumber { get; set; }
    }
}
