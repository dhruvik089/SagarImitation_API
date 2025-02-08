using SagarImitaion.Data.DBRepository.User;
using SagarImitation.Common.Helpers;
using SagarImitation.Model.User;

namespace SagarImitation.Service.User
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<BaseApiResponse> ActiveInactiveUser(long userId)
        {
            return _userRepository.ActiveInactiveUser(userId);
        }

        public Task<BaseApiResponse> DeleteUser(long userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public Task<UserResponseModel> GetUserById(long userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public Task<BaseApiResponse> SaveUser(UserRequestModel model)
        {
            return _userRepository.SaveUser(model);
        }

        public Task<List<UserResponseModel>> UserList()
        {
            return _userRepository.UserList();
        }
    }
}
