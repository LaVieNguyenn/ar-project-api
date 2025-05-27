using ARClothingAPI.DAL.Repositories.UserRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.DAL.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task CommitAsync();
    }
}
