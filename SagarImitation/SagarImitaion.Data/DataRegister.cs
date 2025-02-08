using SagarImitaion.Data.DBRepository.Category;
using SagarImitaion.Data.DBRepository.Product;
using SagarImitaion.Data.DBRepository.User;
using SagarImitation.Data.DBRepository.Account;

namespace SagarImitation.Data
{
    public static class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dataDictionary = new Dictionary<Type, Type>
            {
                { typeof(IAccountRepository), typeof(AccountRepository) },
                { typeof(IProductRepository), typeof(ProductRepository) },
                { typeof(IUserRepository), typeof(UserRepository) },
                { typeof(ICategoryRepository), typeof(CategoryRepository) },
            };
            return dataDictionary;
        }
    }
}