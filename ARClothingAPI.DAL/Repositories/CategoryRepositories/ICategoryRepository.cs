using ARClothingAPI.Common.Entities;

namespace ARClothingAPI.DAL.Repositories.CategoryRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetByParentIdAsync(string? parentId);
    }
}
