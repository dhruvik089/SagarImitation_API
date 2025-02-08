using SagarImitaion.Data.DBRepository.Product;
using SagarImitaion.Model.Product;
using SagarImitation.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagarImitation.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) { _productRepository = productRepository; }

        public Task<BaseApiResponse> DeleteProduct(long productId)
        {
            return _productRepository.DeleteProduct(productId);
        }
        public async Task<BaseApiResponse> ActiveInactiveProduct(long productId)
        {
            return await _productRepository.ActiveInactiveProduct(productId);
        }
        public async Task<ProductResponseModel> GetProductDetailsById(long productId)
        {
            return await _productRepository.GetProductDetailsById(productId);
        }

        public Task<List<ProductResponseModel>> ProductList()
        {
            return _productRepository.ProductList();
        }

        public Task<BaseApiResponse> Save(ProductRequestModel model)
        {
            return _productRepository.Save(model);
        }
    }
}
