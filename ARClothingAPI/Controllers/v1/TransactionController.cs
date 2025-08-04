using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ARClothingAPI.BLL.Services.TransactionServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARClothingAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public Task<ApiResponse<IEnumerable<TransactionDto>>> GetAll()
            => _service.GetAllAsync();

        [HttpGet("{id}")]
        [Authorize]
        public Task<ApiResponse<TransactionDto>> GetById(string id)
            => _service.GetByIdAsync(id);

        [HttpPost]
        [Authorize]
        public Task<ApiResponse<TransactionDto>> Create([FromBody] TransactionCreateDto dto)
        {
            var email = User.FindFirst("email")?.Value!;
            return _service.CreateAsync(dto, email);
        }

        [HttpPut("{id}")]
        [Authorize]
        public Task<ApiResponse<TransactionDto>> Update(string id, [FromBody] TransactionUpdateDto dto)
        {
            var email = User.FindFirst("email")?.Value!;
            return _service.UpdateAsync(id, dto, email);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public Task<ApiResponse<bool>> Delete(string id)
        {
            var email = User.FindFirst("email")?.Value!;
            return _service.DeleteAsync(id, email);
        }
    }
}