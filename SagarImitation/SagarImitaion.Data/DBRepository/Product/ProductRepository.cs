using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Server;
using SagarImitaion.Model.Product;
using SagarImitation.Common.Helpers;
using SagarImitation.Data;
using SagarImitation.Model.Common;
using SagarImitation.Model.Config;
using SagarImitation.Model.Login;
using SagarImitation.Model.Token;
using System.Data;
using System.Reflection;

namespace SagarImitaion.Data.DBRepository.Product
{
    public class ProductRepository : BaseRepository, IProductRepository
    {

        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public ProductRepository(IConfiguration config, IOptions<ConnectionString> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        public async Task<BaseApiResponse> DeleteProduct(long productId)
        {
            var param = new DynamicParameters();
            param.Add("@ProductId", productId);

            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.DeleteProduct, param, commandType: CommandType.StoredProcedure);
        }
        
        public async Task<BaseApiResponse> ActiveInactiveProduct(long productId)
        {
            var param = new DynamicParameters();
            param.Add("@ProductId", productId);

            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.ActiveInactiveProduct, param, commandType: CommandType.StoredProcedure);
        }
          public async Task<ProductResponseModel> GetProductDetailsById(long productId)
        {
            var param = new DynamicParameters();
            param.Add("@ProductId", productId);

            return await QueryFirstOrDefaultAsync<ProductResponseModel>(StoredProcedures.GetProductDetailsById, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<ProductResponseModel>> ProductList()
        {
            var data= await QueryAsync<ProductResponseModel>(StoredProcedures.ProductList, null, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<BaseApiResponse> Save(ProductRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@ProductName", model.ProductName);
            param.Add("@CategoryId", model.ProductCategoryId);
            param.Add("@ProductImageName", model.ProductImageName);
            param.Add("@ProductId", model.ProductId);

            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.SaveProduct, param, commandType: CommandType.StoredProcedure);
        }
    }
}
