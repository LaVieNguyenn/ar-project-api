 using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.TransactionServices
{
    public interface ITransactionService
    {
        Task<ApiResponse<TransactionDto>> CreateAsync(TransactionCreateDto dto, string createdBy);
        Task<ApiResponse<IEnumerable<TransactionDto>>> GetAllAsync();
        Task<ApiResponse<TransactionDto>> GetByIdAsync(string id);
        Task<ApiResponse<TransactionDto>> UpdateAsync(string id, TransactionUpdateDto dto, string updatedBy);
        Task<ApiResponse<bool>> DeleteAsync(string id, string deletedBy);
        Task<ApiResponse<IEnumerable<TransactionDto>>> GetAllByUserIdAsync(string userId);
    }
}
