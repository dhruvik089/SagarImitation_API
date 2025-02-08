using SagarImitation.Common.Helpers;
using SagarImitation.Data.DBRepository.Account;
using SagarImitation.Model.Common;
using SagarImitation.Model.Login;
using SagarImitation.Model.Token;
using SagarImitation.Model.User;

namespace SagarImitation.Service.Account
{
    public class AccountService : IAccountService
    {
        #region Fields
        private readonly IAccountRepository _repository;
        #endregion

        #region Construtor
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        #endregion
        public async Task<LoginDetailModel> Login(LoginRequestModel model)
        {
            return await _repository.Login(model);
        }

        public Task<BaseApiResponse> LogoutUser(long userId, string loginTokenId)
        {
            return _repository.LogoutUser(userId, loginTokenId);
        }

        public async Task<long> UpdateLoginToken(AccessTokenModel model, long UserId)
        {
            return await _repository.UpdateLoginToken(model, UserId);
        }

        public async Task<ForgetPasswordResponseModel> UserDetailsByEmail(ForgetPasswordRequestModel model)
        {
            return await _repository.UserDetailsByEmail(model);
        }
        public async Task<BaseApiResponse> ResetPassword(ResetPasswordWithLinkRequestModel model)
        {
            return await _repository.ResetPassword(model);
        }
    }
}
