using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SagarImitation.Common.Helpers;
using SagarImitation.Model.User;
using SagarImitation.Service.User;

namespace SagarImitation.API.Areas.Admin
{
    [Route("api/admin/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<UserResponseModel>> List()
        {
            ApiResponse<UserResponseModel> response = new ApiResponse<UserResponseModel>();
            var data = await _userServices.UserList();

            response.Data = data;
            response.Success = true;
            return response;
        }

        [HttpGet("getUserById/{userId}")]
        public async Task<ApiPostResponse<UserResponseModel>> GetUserById(long userId)
        {
            ApiPostResponse<UserResponseModel> response = new ApiPostResponse<UserResponseModel>();
            var data = await _userServices.GetUserById(userId);
            if (data != null)
            {

                response.Data = data;
                response.Success = true;
            }
            response.Success = false;
            return response;
        }
        [HttpDelete("delete/{userId}")]
        public async Task<BaseApiResponse> DeleteUser(long userId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _userServices.DeleteUser(userId);
            response.Success = true;
            response.Message = data.Message;
            return response;
        }
        [HttpGet("active-inactive-user/{userId}")]
        public async Task<BaseApiResponse> ActiveInactiveUser(long userId)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _userServices.ActiveInactiveUser(userId);
            response.Success = true;
            response.Message = data.Message;
            return response;
        }
        [HttpPost("save")]
        public async Task<BaseApiResponse> Save(UserRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            var data = await _userServices.SaveUser(model);
            if (data.Success)
            {
                response.Success = true;
                response.Message = data.Message;
            }
            else
            {
                response.Success = false;
                response.Message = data.Message;
            }
            return response;
        }
    }
}
