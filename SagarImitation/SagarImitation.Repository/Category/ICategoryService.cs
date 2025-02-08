using SagarImitaion.Model.Category;
using SagarImitation.Common.Helpers;

namespace SagarImitation.Service.Category
{
    public interface ICategoryService
    {
        public Task<BaseApiResponse> Save(CategoryRequestModel model);
        public Task<BaseApiResponse> DeleteCategory(long categoryId);
        public Task<List<CategoryResponseModel>> CategoryList();
        public Task<BaseApiResponse> ActiveInactiveCategory(long categoryId);
        public Task<CategoryResponseModel> GetCategoryDetailsById(long categoryId);
        public Task<List<CategoryResponseModel>> GetCategoryListForDropdown();
    }
}
