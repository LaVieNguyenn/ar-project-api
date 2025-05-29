using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.DAL.Database.Storage;
using MongoDB.Driver;

namespace ARClothingAPI.DAL.Repositories.ProductRepositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(IStorageDbContext db) : base(db.Database, ConstData.Collection.Products) { }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCreatedByAsync(string userId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.CreatedBy, userId);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
