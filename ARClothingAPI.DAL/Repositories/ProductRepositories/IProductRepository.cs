using ARClothingAPI.Common.Entities;

namespace ARClothingAPI.DAL.Repositories.ProductRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId);
        Task<IEnumerable<Product>> GetByCreatedByAsync(string userId);
    }
}
