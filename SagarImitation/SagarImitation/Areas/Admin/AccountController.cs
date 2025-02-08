using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SagarImitation.Common.CommonMethod;
using SagarImitation.Common.EmailNotification;
using SagarImitation.Common.Helpers;
using SagarImitation.Model.Login;
using SagarImitation.Model.Settings;
using SagarImitation.Model.Token;
using SagarImitation.Model.User;
using SagarImitation.Service.Account;
using SagarImitation.Service.JWTAuthentication;
using static SagarImitation.Common.EmailNotification.EmailNotification;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;

namespace SagarImitation.API.Areas.Admin
{
    [Route("api/admin/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _config;
        private readonly SMTPSettings _smtpSettings;
        private readonly EmailTemplates _emailTemplates;
        private readonly DataConfig _dataConfig;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(
             IAccountService accountService,
             IConfiguration config,
             IOptions<AppSettings> appSettings,
             IOptions<SMTPSettings> smtpSettings,
             IOptions<EmailTemplates> emailTemplates,
             IOptions<DataConfig> dataConfig,
             IHttpContextAccessor httpContextAccessor,
             IJWTAuthenticationService jwtAuthenticationService,
             IWebHostEnvironment hostingEnvironment
             )
        {
            _accountService = accountService;
            _config = config;
            _appSettings = appSettings.Value;
            _smtpSettings = smtpSettings.Value;
            _emailTemplates = emailTemplates.Value;
            _dataConfig = dataConfig.Value;
            _hostingEnvironment = hostingEnvironment;
            //_httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }

        [HttpPost("login")]
        public async Task<ApiPostResponse<LoginDetailModel>> Login([FromBody] LoginRequestModel model)
        {
            ApiPostResponse<LoginDetailModel> response = new ApiPostResponse<LoginDetailModel>();

            var result = await _accountService.Login(model);
            if (result.UserId != null || result.UserId != 0)
            {
                TokenModel objTokenData = new TokenModel();
                objTokenData.EmailId = model.EmailId;
                objTokenData.Id = result.UserId;
                objTokenData.FullName = result.FullName;

                AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);
                result.JWTToken = objAccessTokenData.Token;
                await _accountService.UpdateLoginToken(objAccessTokenData, objAccessTokenData.UserId);

                response.Data = result;
                response.Message = "Login successfull";
                response.Success = true;
                return response;
            }

            response.Success = false;
            response.Message = result.Message;
            return response;
        }

        [HttpPost("logout/{loginTokenId}")]
        [Authorize]
        public async Task<BaseApiResponse> Logout(string loginTokenId)
        {
            BaseApiResponse response = new BaseApiResponse();

            TokenModel userTokenData = _jwtAuthenticationService.GetUserTokenData();

            var data = await _accountService.LogoutUser(userTokenData.Id, loginTokenId);

            response.Message = data.Message;
            if (data.Success)
            {
                response.Success = true;
                return response;
            }
            response.Success = false;
            return response;
        }

        [HttpPost("forget-password")]
        public async Task<ApiPostResponse<ForgetPasswordResponseModel>> ForgetPassword(ForgetPasswordRequestModel model)
        {
            ApiPostResponse<ForgetPasswordResponseModel> response = new ApiPostResponse<ForgetPasswordResponseModel>();

            var result = await _accountService.UserDetailsByEmail(model);

            if (result == null)
            {
                response.Success = false;
                response.Message = ErrorMessages.InvalidEmailId;
                return response;
            }
            if (result.UserId > 0)
            {
                EmailNotification.EmailSetting setting = new EmailSetting
                {
                    EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                    EmailHostName = _smtpSettings.EmailHostName,
                    EmailPassword = _smtpSettings.EmailPassword,
                    EmailAppPassword = _smtpSettings.EmailAppPassword,
                    EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                    FromEmail = _smtpSettings.FromEmail,
                    FromName = _smtpSettings.FromName,
                    EmailUsername = _smtpSettings.EmailUsername,
                };
                string EncryptedUserId = HttpUtility.UrlEncode(EncryptionDecryption.GetEncrypt(result.UserId.ToString()));
                string EncryptedEmailId = HttpUtility.UrlEncode(EncryptionDecryption.GetEncrypt(model.EmailId));

                string emailBody = string.Empty;
                bool isSuccess = false;
                string LastForgotPasswordSend = Convert.ToString(String.Format("{0:yyyy-MM-dd HH:mm:ss}", result.LastForgetPasswordSend));
                string LastForgotPasswordDateTime = HttpUtility.UrlEncode(EncryptionDecryption.GetEncrypt(LastForgotPasswordSend));
                Console.WriteLine(_emailTemplates.UserForgotPasswordWithLink);
                string BasePath = Path.Combine(_hostingEnvironment.WebRootPath, _emailTemplates.UserForgotPasswordWithLink);
                string host = _httpContextAccessor?.HttpContext?.Request.Scheme + "://" + _httpContextAccessor?.HttpContext?.Request.Host.Value + "/";

                using (StreamReader reader = new(BasePath))
                {
                    emailBody = reader.ReadToEnd();
                }

                TimeSpan spWorkMin = TimeSpan.FromMinutes(_appSettings.PasswordLinkValidityMins);
                string HoursMinutes = "";
                if ((int)spWorkMin.TotalMinutes > 60)
                {
                    if (spWorkMin.Minutes == 0)
                    {
                        HoursMinutes = (int)spWorkMin.TotalHours + " Hours ";
                    }
                    else
                    {
                        HoursMinutes = (int)spWorkMin.TotalHours + " Hours " + spWorkMin.Minutes + " minutes";
                    }
                }
                else
                {
                    HoursMinutes = spWorkMin.Minutes + " minutes";
                }

                emailBody = emailBody.Replace("##userName##", result.FullName?.ToString());
                emailBody = emailBody.Replace("##EmailId##", result.EmailId);
                emailBody = emailBody.Replace("##TimeValid##", HoursMinutes);

                string PasswordValid = HttpUtility.UrlEncode(EncryptionDecryption.GetEncrypt(_appSettings.PasswordLinkValidityMins.ToString()));
                var refererUrl = _httpContextAccessor?.HttpContext?.Request.Headers["Referer"].ToString();

                emailBody = emailBody.Replace("##ResetPasswordLink##", refererUrl + "reset-password/" + EncryptedUserId + "/" + PasswordValid);
                emailBody = emailBody.Replace("##Year##", DateTime.UtcNow.ToString("yyyy"));
                isSuccess = await Task.Run(() => SendMailMessage(model.EmailId, null, null, Constants.EmailSubject, emailBody, setting, null));

                if (isSuccess)
                {
                    response.Message = ErrorMessages.ForgotPasswordSuccessWithLink;
                    response.Success = true;
                }
                else
                {
                    response.Message = ErrorMessages.ForgetPasswordError;
                    response.Success = false;
                }
                return response;

            }
            else
            {
                response.Message = ErrorMessages.UserError;
                response.Success = false;
                return response;
            }
        }
        [HttpPost("reset-password")]
        public async Task<BaseApiResponse> ResetPassword(ResetPasswordWithLinkRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            var EncryptedUserId = model.UserId;
            if (!string.IsNullOrEmpty(EncryptedUserId))
            {
                long UserId = Convert.ToInt64(EncryptionDecryption.GetDecrypt(model.UserId));
                model.EncryptedURLDateTime = EncryptionDecryption.GetDecrypt(model.EncryptedURLDateTime);
                int DecryptedURLDateTime = Convert.ToInt32(model.EncryptedURLDateTime);
                int myNegInt = System.Math.Abs(DecryptedURLDateTime) * (-1);
                if (model.NewPassword == model.ConfirmPassword)
            {
                var data = await _accountService.ResetPassword(model);
                response.Message = data.Message;
                response.Success = true;
                return response;
            }
            response.Success = false;
            return response;
        }


    }
}
