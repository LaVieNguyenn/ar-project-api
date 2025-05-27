using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.Helpers
{
    public static class ConstData
    {
        public class Db
        {
            public const string AuthDb = "auth_ar_clothing";
            public const string StorageDb = "ar_clothing";
        }

        public class Collection
        {
            public const string Users = "users";
            public const string Roles = "roles";
            public const string Products = "products";
            public const string Categories = "categories";
        }
    }
}
