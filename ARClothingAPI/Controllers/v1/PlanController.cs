using System.Collections.Generic;
using System.Threading.Tasks;
using ARClothingAPI.BLL.Services.PlanServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _service;
        public PlanController(IPlanService service) => _service = service;

        [HttpGet]
        //[Authorize]
        public Task<ApiResponse<IEnumerable<PlanDto>>> GetAll() => _service.GetAllAsync();

        [HttpGet("{id}")]
        [Authorize]
        public Task<ApiResponse<PlanDto>> GetById(string id) => _service.GetByIdAsync(id);

        [HttpPost]
        [Authorize]
        public Task<ApiResponse<PlanDto>> Create([FromBody] PlanCreateDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name) ?? "system";
            return _service.CreateAsync(dto, userName);
        }

        [HttpPut("{id}")]
        [Authorize]
        public Task<ApiResponse<PlanDto>> Update(string id, [FromBody] PlanUpdateDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name) ?? "system";
            return _service.UpdateAsync(id, dto, userName);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public Task<ApiResponse<bool>> Delete(string id) => _service.DeleteAsync(id);
    }
}
