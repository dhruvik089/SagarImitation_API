using SagarImitation.Common.Helpers;
using SagarImitation.Data;
using SagarImitation.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagarImitation.Service.User
{
    public interface IUserServices
    {
        public Task<List<UserResponseModel>> UserList();
        public Task<UserResponseModel> GetUserById(long userId);
        public Task<BaseApiResponse> DeleteUser(long userId);
        public Task<BaseApiResponse> ActiveInactiveUser(long userId);
        public Task<BaseApiResponse> SaveUser(UserRequestModel model);
    }
}
