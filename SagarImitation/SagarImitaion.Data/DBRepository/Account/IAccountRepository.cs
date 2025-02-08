
using SagarImitation.Common.Helpers;
using SagarImitation.Model.Login;
using SagarImitation.Model.Token;

namespace SagarImitation.Data.DBRepository.Account
{
    public interface IAccountRepository
    {
        public Task<LoginDetailModel> Login(LoginRequestModel loginRequest);
        public Task<long> UpdateLoginToken(AccessTokenModel model, long UserId);
        public Task<BaseApiResponse> LogoutUser(long userId,string loginTokenId);
        public Task<ForgetPasswordResponseModel> UserDetailsByEmail(ForgetPasswordRequestModel model);
        public Task<BaseApiResponse> ResetPassword(ResetPasswordWithLinkRequestModel model);
    }
}