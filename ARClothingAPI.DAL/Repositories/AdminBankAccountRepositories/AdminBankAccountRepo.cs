using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.DAL.Database.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.DAL.Repositories.AdminBankAccountRepositories
{
    public class AdminBankAccountRepo : GenericRepository<AdminBankAccount>, IAdminBankAccountRepo
    {
        public AdminBankAccountRepo(IAuthDbContext db) : base(db.Database, ConstData.Collection.AdminBankAccount) { }
    }
}
