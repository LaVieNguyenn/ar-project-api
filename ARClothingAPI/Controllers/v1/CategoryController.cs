using ARClothingAPI.BLL.Services.CategoryServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CategoryCreateUpdateDto categoryDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId == null)
            //    return Unauthorized(ApiResponse<CategoryDto>.ErrorResult("User not authenticated"));

            return await _categoryService.CreateAsync(categoryDto, userId);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAllCategories()
        {
            return await _categoryService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(string id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(string id, [FromBody] CategoryCreateUpdateDto categoryDto)
        {
            return await _categoryService.UpdateAsync(id, categoryDto);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(string id)
        {
            return await _categoryService.DeleteAsync(id);
        }
    }
}
