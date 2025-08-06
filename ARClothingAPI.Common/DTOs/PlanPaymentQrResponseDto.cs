using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class PlanPaymentQrResponseDto
    {
        public string AdminAccountNumber { get; set; } = null!;
        public string AdminBankCode { get; set; } = null!;
        public string AdminAccountName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string PaymentContent { get; set; } = null!;
        public string QrCode { get; set; } = null!; // Chuỗi raw QR cho VietQR
    }
}
