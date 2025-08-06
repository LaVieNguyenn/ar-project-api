using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.PlanPaymentServices
{
    public interface IPlanPaymentService
    {
        Task<ApiResponse<PlanPaymentQrResponseDto>> GeneratePlanPaymentQrAsync(string userId, string planId);
        Task<ApiResponse<bool>> EnrollPlan(EnrollPlanRequestDto dto, string adminName);

    }
}
