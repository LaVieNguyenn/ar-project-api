using MongoDB.Driver;

namespace ARClothingAPI.DAL.Database.Storage
{
    public interface IStorageDbContext
    {
        IMongoDatabase Database { get; }
    }
}
