
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SagarImitaion.Model.Category;
using SagarImitation.Common.Helpers;
using SagarImitation.Data;
using SagarImitation.Model.Config;
using System.Data;

namespace SagarImitaion.Data.DBRepository.Category
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CategoryRepository(IConfiguration config, IOptions<ConnectionString> dataConfig) : base(dataConfig)
        {
            _config = config;
        }

        public async Task<BaseApiResponse> ActiveInactiveCategory(long categoryId)
        {
            var param = new DynamicParameters();
            param.Add("@CategoryId", categoryId);
            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.ActiveInActiveCategory, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<CategoryResponseModel>> CategoryList()
        {
            var data = await QueryAsync<CategoryResponseModel>(StoredProcedures.CategoryList, null, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<BaseApiResponse> DeleteCategory(long categoryId)
        {
            var param = new DynamicParameters();
            param.Add("@CategoryId", categoryId);
            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.DeleteCategory,param,commandType:CommandType.StoredProcedure);
        }

        public async Task<CategoryResponseModel> GetCategoryDetailsById(long categoryId)
        {
            var param = new DynamicParameters();
            param.Add("@CategoryId", categoryId);
            return await QueryFirstOrDefaultAsync<CategoryResponseModel>(StoredProcedures.CategoryListById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<CategoryResponseModel>> GetCategoryListForDropdown()
        {
            var data = await QueryAsync<CategoryResponseModel>(StoredProcedures.CategotyListForDropDown, null, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<BaseApiResponse> Save(CategoryRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@CategoryId", model.CategoryId);
            param.Add("@CategoryName", model.CategoryName);
            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.SaveCategory, param, commandType: CommandType.StoredProcedure);
        }
        #endregion
    }
}
