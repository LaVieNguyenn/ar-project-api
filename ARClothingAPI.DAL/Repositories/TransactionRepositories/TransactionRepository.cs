using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.DAL.Database.Storage;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.DAL.Repositories.TransactionRepositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IStorageDbContext db) : base(db.Database, ConstData.Collection.Transactions)
        { }

        public async Task<IEnumerable<Transaction>> GetAllByUserIdAsync(string userId)
        {
            var filter = Builders<Transaction>.Filter.Eq(x => x.UserId, userId);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
