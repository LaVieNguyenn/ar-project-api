using MongoDB.Driver;

namespace ARClothingAPI.DAL.Database.Auth
{
    public interface IAuthDbContext
    {
        IMongoDatabase Database { get; }
    }
}
