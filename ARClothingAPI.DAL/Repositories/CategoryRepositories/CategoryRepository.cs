using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.DAL.Database.Storage;
using MongoDB.Driver;

namespace ARClothingAPI.DAL.Repositories.CategoryRepositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IStorageDbContext db) : base(db.Database, ConstData.Collection.Categories) { }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(string? parentId)
        {
            var filter = parentId == null
                ? Builders<Category>.Filter.Eq(c => c.ParentCategoryId, null)
                : Builders<Category>.Filter.Eq(c => c.ParentCategoryId, parentId);

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
