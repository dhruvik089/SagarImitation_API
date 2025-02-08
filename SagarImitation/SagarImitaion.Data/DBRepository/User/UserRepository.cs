using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SagarImitation.Common.Helpers;
using SagarImitation.Data;
using SagarImitation.Model.Config;
using SagarImitation.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagarImitaion.Data.DBRepository.User
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public UserRepository(IConfiguration config, IOptions<ConnectionString> dataConfig) : base(dataConfig)
        {
            _config = config;
        }

        public async Task<List<UserResponseModel>> UserList()
        {
            var data = await QueryAsync<UserResponseModel>(StoredProcedures.GetUserList, null, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        public Task<UserResponseModel> GetUserById(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return QueryFirstOrDefaultAsync<UserResponseModel>(StoredProcedures.GetUserById, param, commandType: CommandType.StoredProcedure);
        }
        public Task<BaseApiResponse> DeleteUser(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.DeleteUser, param, commandType: CommandType.StoredProcedure);
        }
        public Task<BaseApiResponse> ActiveInactiveUser(long userId) {

            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.ActiveInActiveUser, param, commandType: CommandType.StoredProcedure);
        }
        public Task<BaseApiResponse> SaveUser(UserRequestModel model) {
            var param = new DynamicParameters();
            param.Add("@UserId", model.UserId);
            param.Add("@FirstName", model.FirstName);
            param.Add("@LastName", model.LastName);
            param.Add("@Email", model.EmailId);
            param.Add("@Password", model.Password);
             
            return QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.SaveUser, param, commandType: CommandType.StoredProcedure);
        }
        #endregion


    }
}
