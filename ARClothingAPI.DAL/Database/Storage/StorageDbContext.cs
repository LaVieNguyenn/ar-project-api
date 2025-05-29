using ARClothingAPI.Common.Helpers;
using MongoDB.Driver;

namespace ARClothingAPI.DAL.Database.Storage
{
    public class StorageDbContext : IStorageDbContext
    {
        public IMongoDatabase Database { get; }

        public StorageDbContext(IMongoClient client)
        {
            Database = client.GetDatabase(ConstData.Db.StorageDb);
        }
    }
}
