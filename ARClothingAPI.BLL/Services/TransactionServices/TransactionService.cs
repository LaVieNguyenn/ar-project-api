using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;

namespace ARClothingAPI.BLL.Services.TransactionServices
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _uow;

        public TransactionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ApiResponse<TransactionDto>> CreateAsync(TransactionCreateDto dto, string createdBy)
        {
            var user = await _uow.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                return ApiResponse<TransactionDto>.ErrorResult("User không tồn tại!");

            var transaction = new Transaction
            {
                Username = user.Username,
                Avatar = user.AvatarUrl ?? "",
                UserId = user.Id,
                Amount = dto.Amount,
                Type = dto.Type,
                Method = dto.Method,
                Note = dto.Note,
                CreatedBy = createdBy,
                UpdatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _uow.Transactions.InsertAsync(transaction);

            if (dto.Type.ToLower() == "topup")
            {
                user.VirtualBalance += dto.Amount;
                user.UpdatedAt = DateTime.UtcNow;
                await _uow.Users.UpdateAsync(user.Id, user);
            }

            await _uow.CommitAsync();

            return ApiResponse<TransactionDto>.SuccessResult(ToDto(transaction), "Nạp tiền thành công!");
        }
        public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetAllAsync()
        {
            var entities = await _uow.Transactions.GetAllAsync();
            var dtos = entities.Select(e => new TransactionDto
            {
                Id = e.Id,
                Username = e.Username,
                Avatar = e.Avatar,
                UserId = e.UserId,
                Amount = e.Amount,
                Type = e.Type,
                Method = e.Method,
                Note = e.Note,
                CreatedBy = e.CreatedBy,
                CreatedAt = e.CreatedAt,
                UpdatedBy = e.UpdatedBy,
                UpdatedAt = e.UpdatedAt
            });

            return ApiResponse<IEnumerable<TransactionDto>>.SuccessResult(dtos);
        }

        public async Task<ApiResponse<TransactionDto>> GetByIdAsync(string id)
        {
            var e = await _uow.Transactions.GetByIdAsync(id);
            if (e == null)
                return ApiResponse<TransactionDto>.ErrorResult("Transaction not found");

            var dto = new TransactionDto
            {
                Id = e.Id,
                Username = e.Username,
                Avatar = e.Avatar,
                UserId = e.UserId,
                Amount = e.Amount,
                Type = e.Type,
                Method = e.Method,
                Note = e.Note,
                CreatedBy = e.CreatedBy,
                CreatedAt = e.CreatedAt,
                UpdatedBy = e.UpdatedBy,
                UpdatedAt = e.UpdatedAt
            };
            return ApiResponse<TransactionDto>.SuccessResult(dto);
        }

        public async Task<ApiResponse<TransactionDto>> UpdateAsync(string id, TransactionUpdateDto dto, string updatedBy)
        {
            var transaction = await _uow.Transactions.GetByIdAsync(id);
            if (transaction == null)
                return ApiResponse<TransactionDto>.ErrorResult("Không tìm thấy giao dịch!");

            var user = await _uow.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                return ApiResponse<TransactionDto>.ErrorResult("User không tồn tại!");

            // Rollback số tiền cũ nếu là topup
            if (transaction.Type.ToLower() == "topup")
                user.VirtualBalance -= transaction.Amount;

            transaction.Amount = dto.Amount;
            transaction.Note = dto.Note;
            transaction.Type = dto.Type;
            transaction.Method = dto.Method;
            transaction.UpdatedBy = updatedBy;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _uow.Transactions.UpdateAsync(id, transaction);

            // Cộng lại số tiền mới nếu là topup
            if (dto.Type.ToLower() == "topup")
            {
                user.VirtualBalance += dto.Amount;
                user.UpdatedAt = DateTime.UtcNow;
                await _uow.Users.UpdateAsync(user.Id, user);
            }

            await _uow.CommitAsync();

            return ApiResponse<TransactionDto>.SuccessResult(ToDto(transaction), "Cập nhật giao dịch thành công!");
        }
        public async Task<ApiResponse<bool>> DeleteAsync(string id, string deletedBy)
        {
            var transaction = await _uow.Transactions.GetByIdAsync(id);
            if (transaction == null)
                return ApiResponse<bool>.ErrorResult("Không tìm thấy giao dịch!");

            var user = await _uow.Users.GetByIdAsync(transaction.UserId);
            if (user == null)
                return ApiResponse<bool>.ErrorResult("User không tồn tại!");

            // Nếu là nạp tiền (topup) thì rollback lại số dư
            if (transaction.Type.ToLower() == "topup")
            {
                user.VirtualBalance -= transaction.Amount;
                if (user.VirtualBalance < 0) user.VirtualBalance = 0;
                user.UpdatedAt = DateTime.UtcNow;
                await _uow.Users.UpdateAsync(user.Id, user);
            }

            await _uow.Transactions.DeleteAsync(id);
            await _uow.CommitAsync();

            return ApiResponse<bool>.SuccessResult(true, "Xóa giao dịch thành công!");
        }


        private TransactionDto ToDto(Transaction t)
        {
            return new TransactionDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Username = t.Username,
                Avatar = t.Avatar,
                Amount = t.Amount,
                Type = t.Type,
                Method = t.Method,
                Note = t.Note,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy,
                UpdatedAt = t.UpdatedAt,
                UpdatedBy = t.UpdatedBy
            };
        }

        public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetAllByUserIdAsync(string userId)
        {
            var transactions = await _uow.Transactions.GetAllByUserIdAsync(userId);
            var dtos = transactions.Select(ToDto);
            return ApiResponse<IEnumerable<TransactionDto>>.SuccessResult(dtos, "Lấy lịch sử giao dịch thành công!");
        }
    }
}