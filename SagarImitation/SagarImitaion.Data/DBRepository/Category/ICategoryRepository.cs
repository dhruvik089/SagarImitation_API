
using SagarImitaion.Model.Category;
using SagarImitation.Common.Helpers;

namespace SagarImitaion.Data.DBRepository.Category
{
    public interface ICategoryRepository
    {
        public Task<BaseApiResponse> Save(CategoryRequestModel model);
        public Task<BaseApiResponse> DeleteCategory(long categoryId);
        public Task<List<CategoryResponseModel>> CategoryList();
        public Task<BaseApiResponse> ActiveInactiveCategory(long categoryId);
        public Task<CategoryResponseModel> GetCategoryDetailsById(long categoryId);
        public Task<List<CategoryResponseModel>> GetCategoryListForDropdown();
    }
}
