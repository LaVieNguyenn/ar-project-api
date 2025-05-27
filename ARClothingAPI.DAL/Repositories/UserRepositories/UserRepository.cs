using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.DAL.Database.Auth;
using MongoDB.Driver;

namespace ARClothingAPI.DAL.Repositories.UserRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IAuthDbContext db) : base(db.Database, ConstData.Collection.Users) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _collection.Find(filter).AnyAsync();
        }
    }
}
