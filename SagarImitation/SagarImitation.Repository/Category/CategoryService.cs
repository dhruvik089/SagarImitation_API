using SagarImitaion.Data.DBRepository.Category;
using SagarImitaion.Model.Category;
using SagarImitation.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagarImitation.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task<BaseApiResponse> ActiveInactiveCategory(long categoryId)
        {
            return _categoryRepository.ActiveInactiveCategory(categoryId);
        }

        public Task<List<CategoryResponseModel>> CategoryList()
        {
            return _categoryRepository.CategoryList();
        }

        public Task<BaseApiResponse> DeleteCategory(long categoryId)
        {
            return _categoryRepository.DeleteCategory(categoryId);
        }

        public Task<CategoryResponseModel> GetCategoryDetailsById(long categoryId)
        {
            return _categoryRepository.GetCategoryDetailsById(categoryId);
        }

        public Task<List<CategoryResponseModel>> GetCategoryListForDropdown()
        {
            return _categoryRepository.GetCategoryListForDropdown();
        }

        public Task<BaseApiResponse> Save(CategoryRequestModel model)
        {
            return _categoryRepository.Save(model);
        }
    }
}
