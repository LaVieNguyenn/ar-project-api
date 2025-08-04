using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.PlanServices
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _uow;
        public PlanService(IUnitOfWork uow) => _uow = uow;

        public async Task<ApiResponse<PlanDto>> CreateAsync(PlanCreateDto dto, string userName)
        {
            var now = DateTime.UtcNow;
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                DurationMonths = dto.DurationMonths,
                Price = dto.Price,
                DiscountPercent = dto.DiscountPercent,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = userName,
                UpdatedAt = now,
                UpdatedBy = userName
            };
            await _uow.Plans.InsertAsync(plan);
            await _uow.CommitAsync();
            return ApiResponse<PlanDto>.SuccessResult(ToDto(plan));
        }

        public async Task<ApiResponse<IEnumerable<PlanDto>>> GetAllAsync()
        {
            var items = await _uow.Plans.GetAllAsync();
            return ApiResponse<IEnumerable<PlanDto>>.SuccessResult(items.Select(ToDto));
        }

        public async Task<ApiResponse<PlanDto>> GetByIdAsync(string id)
        {
            var p = await _uow.Plans.GetByIdAsync(id);
            if (p == null) return ApiResponse<PlanDto>.ErrorResult("Plan not found");
            return ApiResponse<PlanDto>.SuccessResult(ToDto(p));
        }

        public async Task<ApiResponse<PlanDto>> UpdateAsync(string id, PlanUpdateDto dto, string userName)
        {
            var p = await _uow.Plans.GetByIdAsync(id);
            if (p == null) return ApiResponse<PlanDto>.ErrorResult("Plan not found");
            p.Name = dto.Name;
            p.Description = dto.Description;
            p.DurationMonths = dto.DurationMonths;
            p.Price = dto.Price;
            p.DiscountPercent = dto.DiscountPercent;
            p.UpdatedAt = DateTime.UtcNow;
            p.UpdatedBy = userName;
            await _uow.Plans.UpdateAsync(id, p);
            await _uow.CommitAsync();
            return ApiResponse<PlanDto>.SuccessResult(ToDto(p));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            await _uow.Plans.DeleteAsync(id);
            await _uow.CommitAsync();
            return ApiResponse<bool>.SuccessResult(true, "Plan deleted");
        }

        private PlanDto ToDto(Plan p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            DurationMonths = p.DurationMonths,
            Price = p.Price,
            DiscountPercent = p.DiscountPercent,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedBy,
            UpdatedAt = p.UpdatedAt,
            UpdatedBy = p.UpdatedBy
        };
    }
}
