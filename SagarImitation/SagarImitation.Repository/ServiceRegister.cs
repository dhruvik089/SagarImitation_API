using SagarImitation.Service.Account;
using SagarImitation.Service.Category;
using SagarImitation.Service.Product;
using SagarImitation.Service.User;

namespace SagarImitation.Service
{
    public static class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictonary = new Dictionary<Type, Type>
            {
                { typeof(IAccountService), typeof(AccountService) },
                { typeof(IProductService), typeof(ProductService) },
                { typeof(IUserServices), typeof(UserServices) },
                { typeof(ICategoryService), typeof(CategoryService) },
            };
            return serviceDictonary;
        }
    }
}