using ARClothingAPI.BLL.Services.TransactionServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _uow;

    public TransactionService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    // Chỉ dùng cho admin Enroll (KHÔNG tự gọi khi user scan QR)
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
            Type = "plan_purchase",
            Method = "bank_transfer",
            Note = dto.Note,
            PlanId = dto.PlanId,
            PaymentContent = dto.PaymentContent,
            BankName = dto.BankName,
            BankRefNumber = dto.BankRefNumber,
            Status = "approved", // luôn approved vì chỉ tạo khi đã xác thực
            CreatedBy = createdBy,
            UpdatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _uow.Transactions.InsertAsync(transaction);
        await _uow.CommitAsync();

        return ApiResponse<TransactionDto>.SuccessResult(ToDto(transaction), "Ghi nhận giao dịch thành công!");
    }

    public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetAllAsync()
    {
        var entities = await _uow.Transactions.GetAllAsync();
        var dtos = entities.Select(ToDto);
        return ApiResponse<IEnumerable<TransactionDto>>.SuccessResult(dtos);
    }

    public async Task<ApiResponse<TransactionDto>> GetByIdAsync(string id)
    {
        var e = await _uow.Transactions.GetByIdAsync(id);
        if (e == null)
            return ApiResponse<TransactionDto>.ErrorResult("Transaction not found");
        return ApiResponse<TransactionDto>.SuccessResult(ToDto(e));
    }

    // Update chỉ dùng cho nghiệp vụ chỉnh sửa thông tin giao dịch đã duyệt (admin)
    public async Task<ApiResponse<TransactionDto>> UpdateAsync(string id, TransactionUpdateDto dto, string updatedBy)
    {
        var transaction = await _uow.Transactions.GetByIdAsync(id);
        if (transaction == null)
            return ApiResponse<TransactionDto>.ErrorResult("Không tìm thấy giao dịch!");

        transaction.Amount = dto.Amount;
        transaction.Note = dto.Note;
        transaction.Method = dto.Method;
        transaction.PlanId = dto.PlanId;
        transaction.PaymentContent = dto.PaymentContent;
        transaction.BankName = dto.BankName;
        transaction.BankRefNumber = dto.BankRefNumber;
        transaction.UpdatedBy = updatedBy;
        transaction.UpdatedAt = DateTime.UtcNow;

        await _uow.Transactions.UpdateAsync(id, transaction);
        await _uow.CommitAsync();

        return ApiResponse<TransactionDto>.SuccessResult(ToDto(transaction), "Cập nhật giao dịch thành công!");
    }

    // Xóa chỉ là xóa transaction, không ảnh hưởng gì tới số dư user
    public async Task<ApiResponse<bool>> DeleteAsync(string id, string deletedBy)
    {
        var transaction = await _uow.Transactions.GetByIdAsync(id);
        if (transaction == null)
            return ApiResponse<bool>.ErrorResult("Không tìm thấy giao dịch!");

        await _uow.Transactions.DeleteAsync(id);
        await _uow.CommitAsync();

        return ApiResponse<bool>.SuccessResult(true, "Xóa giao dịch thành công!");
    }

    private TransactionDto ToDto(Transaction t) => new()
    {
        Id = t.Id,
        UserId = t.UserId,
        Username = t.Username,
        Avatar = t.Avatar,
        Amount = t.Amount,
        Type = t.Type,
        Method = t.Method,
        Note = t.Note,
        PlanId = t.PlanId,
        PaymentContent = t.PaymentContent,
        BankName = t.BankName,
        BankRefNumber = t.BankRefNumber,
        Status = t.Status,
        CreatedAt = t.CreatedAt,
        CreatedBy = t.CreatedBy,
        UpdatedAt = t.UpdatedAt,
        UpdatedBy = t.UpdatedBy
    };

    public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetAllByUserIdAsync(string userId)
    {
        var transactions = await _uow.Transactions.GetAllByUserIdAsync(userId);
        var dtos = transactions.Select(ToDto);
        return ApiResponse<IEnumerable<TransactionDto>>.SuccessResult(dtos, "Lấy lịch sử giao dịch thành công!");
    }
}
