using ARClothingAPI.DAL.Database.Auth;
using ARClothingAPI.DAL.Database.Storage;
using ARClothingAPI.DAL.Repositories.AdminBankAccountRepositories;
using ARClothingAPI.DAL.Repositories.CartRepositories;
using ARClothingAPI.DAL.Repositories.CategoryRepositories;
using ARClothingAPI.DAL.Repositories.PlanRepositories;
using ARClothingAPI.DAL.Repositories.ProductRepositories;
using ARClothingAPI.DAL.Repositories.TransactionRepositories;
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
        private ICartRepository? _carts;
        private ITransactionRepository? _transactions;
        private IPlanRepository? _plans;
        private IAdminBankAccountRepo? _adminBankAccounts;

        public UnitOfWork(IAuthDbContext authDb, IStorageDbContext storageDb)
        {
            _authDb = authDb;
            _storageDb = storageDb;
        }

        public IUserRepository Users => _users ??= new UserRepository(_authDb);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_storageDb);
        public IProductRepository Products => _products ??= new ProductRepository(_storageDb);
        public ICartRepository Carts => _carts ??= new CartRepository(_storageDb);
        public ITransactionRepository Transactions => _transactions ??= new TransactionRepository(_storageDb);
        public IPlanRepository Plans => _plans ??= new PlanRepository(_storageDb);
        public IAdminBankAccountRepo AdminBankAccounts => _adminBankAccounts ??= new AdminBankAccountRepo(_authDb);

        public Task CommitAsync() => Task.CompletedTask;
        public void Dispose() { }
    }
}
