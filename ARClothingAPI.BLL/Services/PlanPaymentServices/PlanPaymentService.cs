using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;
using QRCoder;
using System.Net.Http.Json;
using System.Security.Claims;
using ZXing.QrCode.Internal;
using static ARClothingAPI.Common.Helpers.ConstData;

namespace ARClothingAPI.BLL.Services.PlanPaymentServices
{
    public class PlanPaymentService : IPlanPaymentService
    {
        private IUnitOfWork _uow;
        public PlanPaymentService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<ApiResponse<PlanPaymentQrResponseDto>> GeneratePlanPaymentQrAsync(string userId, string planId)
        {
            var plan = await _uow.Plans.GetByIdAsync(planId);
            if (plan == null || !plan.IsActive)
                return ApiResponse<PlanPaymentQrResponseDto>.ErrorResult("Plan không hợp lệ!");

            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<PlanPaymentQrResponseDto>.ErrorResult("User không tồn tại!");

            var adminBank = _uow.AdminBankAccounts.GetAllAsync().Result.FirstOrDefault(x => x.IsActive == true);
            if (adminBank == null)
                return ApiResponse<PlanPaymentQrResponseDto>.ErrorResult("Không tìm thấy tài khoản nhận tiền!");

            var amount = plan.Price * (1 - plan.DiscountPercent / 100);

            // Generate unique content để admin tracking
            var paymentContent = $"PLAN {user.Id} {plan.Id} {DateTime.UtcNow:yyyyMMddHHmmss}";

            // Gọi VietQR API lấy chuỗi raw qrCode
            var payload = new
            {
                accountNo = adminBank.AccountNumber,
                accountName = adminBank.AccountName,
                acqId = adminBank.BankCode, // mã ngân hàng Napas
                amount = amount,
                addInfo = paymentContent,
                format = "text"
            };
            using var http = new HttpClient();
            var resp = await http.PostAsJsonAsync("https://api.vietqr.io/v2/generate", payload);
            if (!resp.IsSuccessStatusCode)
                return ApiResponse<PlanPaymentQrResponseDto>.ErrorResult("Không thể kết nối VietQR!");

            var result = await resp.Content.ReadFromJsonAsync<VietQrApiResponse>();
            
            if (result == null || result.code != "00")
                return ApiResponse<PlanPaymentQrResponseDto>.ErrorResult("Không generate được QR!");

            return ApiResponse<PlanPaymentQrResponseDto>.SuccessResult(new PlanPaymentQrResponseDto
            {
                AdminAccountNumber = adminBank.AccountNumber,
                AdminBankCode = adminBank.BankCode,
                AdminAccountName = adminBank.AccountName,
                Amount = amount,
                PaymentContent = paymentContent,
                QrCode = result.data.qrCode 
            }, "Vui lòng chuyển khoản đúng nội dung này!");
        }

        public class VietQrApiResponse
        {
            public string code { get; set; } = null!;
            public string desc { get; set; } = null!;
            public VietQrData data { get; set; } = null!;
            public class VietQrData
            {
                public string qrCode { get; set; } = null!;
                public string? qrDataURL { get; set; }
            }
        }

        public async Task<ApiResponse<bool>> EnrollPlan(EnrollPlanRequestDto dto, string adminName)
        {
            var user = await _uow.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                return ApiResponse<bool>.ErrorResult("User không tồn tại!");

            var plan = await _uow.Plans.GetByIdAsync(dto.PlanId);
            if (plan == null || !plan.IsActive)
                return ApiResponse<bool>.ErrorResult("Plan không hợp lệ!");

            var now = DateTime.UtcNow;
            var transaction = new Transaction
            {
                Username = user.Username,
                Avatar = user.AvatarUrl ?? "",
                UserId = user.Id,
                Amount = dto.Amount,
                Type = "plan_purchase",
                Method = "bank_transfer",
                Note = $"Mua/gia hạn plan '{plan.Name}' (Admin duyệt)",
                PlanId = plan.Id,
                PaymentContent = dto.PaymentContent,
                BankRefNumber = dto.BankRefNumber,
                BankName = dto.BankName,
                Status = "approved",
                CreatedBy = adminName ?? "admin",
                CreatedAt = now,
                UpdatedBy = adminName ?? "admin",
                UpdatedAt = now
            };
            await _uow.Transactions.InsertAsync(transaction);

            // Enroll plan (reset/gia hạn)
            if (user.PlanId == null || user.PlanEnd == null || user.PlanEnd < now)
            {
                user.PlanId = plan.Id;
                user.PlanStart = now;
                user.PlanEnd = now.AddMonths(plan.DurationMonths);
            }
            else if (user.PlanId == plan.Id)
            {
                user.PlanEnd = user.PlanEnd.Value.AddMonths(plan.DurationMonths);
            }
            else
            {
                user.PlanId = plan.Id;
                user.PlanStart = now;
                user.PlanEnd = now.AddMonths(plan.DurationMonths);
            }
            user.UpdatedAt = now;
            await _uow.Users.UpdateAsync(user.Id, user);

            // (Optional) Insert subscriptions TTL nếu bạn muốn tự động hết hạn

            await _uow.CommitAsync();

            return ApiResponse<bool>.SuccessResult(true, "Duyệt và kích hoạt gói thành công!");
        }

    }
}