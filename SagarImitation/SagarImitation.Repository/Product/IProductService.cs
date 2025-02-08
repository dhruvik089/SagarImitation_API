using SagarImitaion.Model.Product;
using SagarImitation.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagarImitation.Service.Product
{
    public interface IProductService
    {
        public Task<BaseApiResponse> Save(ProductRequestModel model);
        public Task<BaseApiResponse> DeleteProduct(long productId);
        public Task<List<ProductResponseModel>> ProductList();
        public Task<BaseApiResponse> ActiveInactiveProduct(long productId);
        public Task<ProductResponseModel> GetProductDetailsById(long productId);
    }
}
