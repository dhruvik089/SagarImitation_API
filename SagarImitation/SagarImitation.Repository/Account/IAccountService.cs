using SagarImitation.Common.Helpers;
using SagarImitation.Model.Common;
using SagarImitation.Model.Login;
using SagarImitation.Model.Token;
using SagarImitation.Model.User;

namespace SagarImitation.Service.Account
{
    public interface IAccountService 
    {
        public Task<LoginDetailModel> Login(LoginRequestModel model);
        public Task<long> UpdateLoginToken(AccessTokenModel model, long UserId);
        public Task<BaseApiResponse> LogoutUser(long userId, string loginTokenId);
        public Task<ForgetPasswordResponseModel> UserDetailsByEmail(ForgetPasswordRequestModel model);
        public Task<BaseApiResponse> ResetPassword(ResetPasswordWithLinkRequestModel model);
    }
}