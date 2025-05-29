using ARClothingAPI.Common.Helpers;
using MongoDB.Driver;

namespace ARClothingAPI.DAL.Database.Auth
{
    public class AuthDbContext : IAuthDbContext
    {
        public IMongoDatabase Database { get; }

        public AuthDbContext(IMongoClient client)
        {
            Database = client.GetDatabase(ConstData.Db.AuthDb);
        }
    }

}
