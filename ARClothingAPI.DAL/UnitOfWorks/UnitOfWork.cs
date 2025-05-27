using ARClothingAPI.DAL.Database.Auth;
using ARClothingAPI.DAL.Database.Storage;
using ARClothingAPI.DAL.Repositories.UserRepositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.DAL.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAuthDbContext _authDb;
        private readonly IStorageDbContext _storageDb;
        private IUserRepository? _users;

        public UnitOfWork(IAuthDbContext authDb, IStorageDbContext storageDb)
        {
            _authDb = authDb;
            _storageDb = storageDb;
        }

        public IUserRepository Users => _users ??= new UserRepository(_authDb);

        public Task CommitAsync() => Task.CompletedTask;
        public void Dispose() { }
    }
}
