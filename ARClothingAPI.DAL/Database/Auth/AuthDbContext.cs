using ARClothingAPI.Common.Helpers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
