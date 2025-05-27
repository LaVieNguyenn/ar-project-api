using ARClothingAPI.Common.Entities;

namespace ARClothingAPI.DAL.Repositories.UserRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
