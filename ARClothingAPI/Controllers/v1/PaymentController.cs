using ARClothingAPI.BLL.Services.PlanPaymentServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPlanPaymentService _planPaymentService;
        public PaymentController(IPlanPaymentService planPaymentService)
        {
            _planPaymentService = planPaymentService;
        }

        [HttpPost("generate-plan-payment-qr")]
        [Authorize]
        public Task<ApiResponse<PlanPaymentQrResponseDto>> GeneratePlanPaymentQr([FromBody] GeneratePlanPaymentQrDto dto)
        {
            var userId = User.FindFirstValue("id")!;
            return _planPaymentService.GeneratePlanPaymentQrAsync(userId, dto.PlanId);
        }

        [HttpPost("enroll-plan")]
        [Authorize]
        public async Task<ApiResponse<bool>> EnrollPlan([FromBody] EnrollPlanRequestDto dto)
        {
            var userId = User.FindFirstValue("id")!;
            return await _planPaymentService.EnrollPlan(dto, userId);
        }


    }
}
