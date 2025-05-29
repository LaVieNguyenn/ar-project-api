using MongoDB.Bson;
using MongoDB.Driver;

namespace ARClothingAPI.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoDatabase db, string collectionName)
        {
            _collection = db.GetCollection<T>(collectionName);
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
            => await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();

        public virtual async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter)
            => await _collection.Find(filter).ToListAsync();

        public virtual async Task InsertAsync(T entity)
            => await _collection.InsertOneAsync(entity);

        public virtual async Task UpdateAsync(string id, T entity)
            => await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", ObjectId.Parse(id)), entity);

        public virtual async Task DeleteAsync(string id)
            => await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", ObjectId.Parse(id)));
    }
}
