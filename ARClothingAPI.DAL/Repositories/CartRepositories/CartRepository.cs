using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.DAL.Database.Storage;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARClothingAPI.DAL.Repositories.CartRepositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(IStorageDbContext db) : base(db.Database, ConstData.Collection.Carts) { }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
