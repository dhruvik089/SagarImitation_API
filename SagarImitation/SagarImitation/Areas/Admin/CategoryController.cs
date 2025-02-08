using Microsoft.AspNetCore.Mvc;
using SagarImitaion.Model.Category;
using SagarImitation.Common.Helpers;
using SagarImitation.Service.Category;

namespace SagarImitation.API.Areas.Admin
{
    [Route("api/admin/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("save")]
        public async Task<BaseApiResponse> Save(CategoryRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _categoryService.Save(model);

            response.Success = data.Success;
            response.Message = data.Message;
            return response;

        }
        
        [HttpDelete("category-delete/{categoryId}")]
        public async Task<BaseApiResponse> DeleteCategory(long categoryId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _categoryService.DeleteCategory(categoryId);
            response.Success = true;
            response.Message = data.Message;
            return response;
        }
        
        [HttpGet("list")]
        public async Task<ApiResponse<CategoryResponseModel>> CategoryList()
        {
            ApiResponse<CategoryResponseModel> response = new ApiResponse<CategoryResponseModel>();

            var data = await _categoryService.CategoryList();
            response.Data = data;
            response.Success = true;
            return response;
        }

        [HttpGet("active-inactive-category/{categoryId}")]
        public async Task<BaseApiResponse> ActiveInactiveCategory(long categoryId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _categoryService.ActiveInactiveCategory(categoryId);
            response.Success = true;
            response.Message = data.Message;
            return response;
        }

        [HttpGet("getDetailsById/{categoryId}")]
        public async Task<ApiPostResponse<CategoryResponseModel>> GetCategoryDetailsById(long categoryId)
        {
            ApiPostResponse<CategoryResponseModel> response = new ApiPostResponse<CategoryResponseModel>();

            var data =await _categoryService.GetCategoryDetailsById(categoryId);
            if (data != null)
            {
                response.Data = data;
                response.Success = true;
                return response;
            }
            response.Success = false;
            return response;
        }

        [HttpGet("getCategoryListForDropdown")]
        public async Task<ApiResponse<CategoryResponseModel>> CategoryListForDropdown()
        {
            ApiResponse<CategoryResponseModel> response = new ApiResponse<CategoryResponseModel>();

            var data = await _categoryService.GetCategoryListForDropdown();
            response.Data = data;
            response.Success = true;
            return response;
        }

    }
}
