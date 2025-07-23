using ARClothingAPI.DAL.Repositories.CartRepositories;
using ARClothingAPI.DAL.Repositories.CategoryRepositories;
using ARClothingAPI.DAL.Repositories.ProductRepositories;
using ARClothingAPI.DAL.Repositories.UserRepositories;

namespace ARClothingAPI.DAL.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        Task CommitAsync();
    }
}
