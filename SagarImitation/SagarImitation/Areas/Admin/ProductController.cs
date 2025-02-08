using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using SagarImitaion.Model.Product;
using SagarImitation.Common.CommonMethod;
using SagarImitation.Common.Helpers;
using SagarImitation.Service.Product;
using System.Xml.Linq;

namespace SagarImitation.API.Areas.Admin
{
    [Route("api/admin/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IProductService _productService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public ProductController(IConfiguration config, IProductService productService)
        {
            _productService = productService;
            _config = config;
        }
        [HttpPost("save")]
        public async Task<BaseApiResponse> Save([FromBody] ProductRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();

            if (model.ProductImage != null)
            {
                var path = _hostingEnvironment.WebRootPath + _config["Path:ProductImagePath"];
                model.ProductImageName = await CommonMethods.UploadImage(model.ProductImage, path);
            }
            var data = await _productService.Save(model);
            if (data.Success)
            {
                response.Success = true;
                response.Message = data.Message;
                return response;
            }
            response.Success = false;
            response.Message = ErrorMessages.SomethingWentWrong;
            return response;
        }

        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<BaseApiResponse> DeleteProduct(long productId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _productService.DeleteProduct(productId);
            return response;
        }

        [HttpGet("active-inactive-product/{productId}")]
        public async Task<BaseApiResponse> ActiveInactiveProduct(long productId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _productService.ActiveInactiveProduct(productId);
            if (data != null)
            {
                response.Message = data.Message;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }
        [HttpGet("getProductById/{productId}")]
        public async Task<ApiPostResponse<ProductResponseModel>> GetProductById(long productId)
        {
            ApiPostResponse<ProductResponseModel> response = new ApiPostResponse<ProductResponseModel>();
            var result = await _productService.GetProductDetailsById(productId);
            if (result != null)
            {
                response.Data = result;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<ProductResponseModel>> List()
        {
            ApiResponse<ProductResponseModel> response = new ApiResponse<ProductResponseModel>();
            var result = await _productService.ProductList();
            response.Data = result;
            response.Success = true;
            return response;
        }
    }
}
