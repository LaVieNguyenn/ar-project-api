using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.PlanServices
{
    public interface IPlanService
    {
        Task<ApiResponse<PlanDto>> CreateAsync(PlanCreateDto dto, string userName);
        Task<ApiResponse<IEnumerable<PlanDto>>> GetAllAsync();
        Task<ApiResponse<PlanDto>> GetByIdAsync(string id);
        Task<ApiResponse<PlanDto>> UpdateAsync(string id, PlanUpdateDto dto, string userName);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}
