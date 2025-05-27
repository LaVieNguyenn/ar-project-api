using ARClothingAPI.Common.Helpers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
