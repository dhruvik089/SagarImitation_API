using SagarImitaion.Model.Product;
using SagarImitation.Common.Helpers;

namespace SagarImitaion.Data.DBRepository.Product
{
    public interface IProductRepository
    {
        public Task<BaseApiResponse> Save(ProductRequestModel model);
        public Task<BaseApiResponse> DeleteProduct(long productId);
        public Task<List<ProductResponseModel>> ProductList();
        public Task<BaseApiResponse> ActiveInactiveProduct(long productId);
        public Task<ProductResponseModel> GetProductDetailsById(long productId);
    }
}
