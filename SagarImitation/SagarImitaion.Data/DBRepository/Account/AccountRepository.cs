
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Server;
using SagarImitation.Common.Helpers;
using SagarImitation.Data;
using SagarImitation.Model.Common;
using SagarImitation.Model.Config;
using SagarImitation.Model.Login;
using SagarImitation.Model.Token;
using SagarImitation.Model.User;
using System.Data;
using System.Net.Quic;
using System.Reflection;

namespace SagarImitation.Data.DBRepository.Account
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public AccountRepository(IConfiguration config, IOptions<ConnectionString> dataConfig) : base(dataConfig)
        {
            _config = config;
        }

        #endregion
        public async Task<LoginDetailModel> Login(LoginRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Email", model.EmailId);
            param.Add("@Password", model.Password);
            return await QueryFirstOrDefaultAsync<LoginDetailModel>(StoredProcedures.LoginUser, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<BaseApiResponse> LogoutUser(long userId, string loginTokenId)
        {
            var param=new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@Token", loginTokenId);
            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.LogoutUser, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> UpdateLoginToken(AccessTokenModel model, long UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            param.Add("@Token", model.Token);
            param.Add("@ExpiredDate", model.ExpiresOnUTC);
            return await QueryFirstOrDefaultAsync<long>(StoredProcedures.UpdateLoginToken, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<ForgetPasswordResponseModel> UserDetailsByEmail(ForgetPasswordRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Email", model.EmailId);
            return await QueryFirstOrDefaultAsync<ForgetPasswordResponseModel>(StoredProcedures.GetUserIdByEmail, param, commandType: CommandType.StoredProcedure);

        }
        public async Task<BaseApiResponse> ResetPassword(ResetPasswordWithLinkRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Email", model.EmailId);
            param.Add("@Password", model.NewPassword);
            return await QueryFirstOrDefaultAsync<BaseApiResponse>(StoredProcedures.ResetPassword, param, commandType: CommandType.StoredProcedure);

        }

    }
}
