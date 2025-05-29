using ARClothingAPI.DAL.Database.Auth;
using ARClothingAPI.DAL.Database.Storage;
using ARClothingAPI.DAL.Repositories.CategoryRepositories;
using ARClothingAPI.DAL.Repositories.ProductRepositories;
using ARClothingAPI.DAL.Repositories.UserRepositories;

namespace ARClothingAPI.DAL.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAuthDbContext _authDb;
        private readonly IStorageDbContext _storageDb;
        private IUserRepository? _users;
        private ICategoryRepository? _categories;
        private IProductRepository? _products;

        public UnitOfWork(IAuthDbContext authDb, IStorageDbContext storageDb)
        {
            _authDb = authDb;
            _storageDb = storageDb;
        }

        public IUserRepository Users => _users ??= new UserRepository(_authDb);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_storageDb);
        public IProductRepository Products => _products ??= new ProductRepository(_storageDb);

        public Task CommitAsync() => Task.CompletedTask;
        public void Dispose() { }
    }
}
