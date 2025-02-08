﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SagarImitation.Common.CommonMethod;
using SagarImitation.Common.EmailNotification;
using SagarImitation.Common.Helpers;
using System.Diagnostics;
using System.Net;
using static SagarImitation.Common.EmailNotification.EmailNotification;
using System.Text;
using SagarImitation.Model.Settings;
using SagarImitation.Model.Token;
using SagarImitation.Model.ReqResponse;
using SagarImitation.Service.JWTAuthentication;
using SagarImitation.Service.Account;

namespace SagarImitation.API.Middleware
{
    public class CustomMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly SMTPSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private readonly EmailTemplates _emailTemplates;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="memoryCache"></param>
        public CustomMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings,
            IWebHostEnvironment hostingEnvironment, IJWTAuthenticationService jwtAuthenticationService,
            IConfiguration config, IOptions<SMTPSettings> smtpSettings,IOptions<EmailTemplates> emailTemplates
            )
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _jwtAuthenticationService = jwtAuthenticationService;
            _config = config;
            _smtpSettings = smtpSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _emailTemplates = emailTemplates.Value;
        }

        /// <summary>
        /// Invoke on every request response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settingService"></param>
        public async Task Invoke(HttpContext context, IAccountService _accountService)
        {

            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs");

            string headervalue = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(headervalue))
            {
                string jwtToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    TokenModel userTokenModel = _jwtAuthenticationService.GetUserTokenData();
                    if (userTokenModel != null)
                    {
                        if (userTokenModel.TokenValidTo < DateTime.UtcNow.AddMinutes(1))
                        {
                            context.Response.ContentType = Constants.ContentTypeJson;
                            context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = Constants.AccessControlAllowOrigin;
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                    else
                    {
                        context.Response.ContentType = Constants.ContentTypeJson;
                        context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = Constants.AccessControlAllowOrigin;
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }

                }
            }

            using (MemoryStream requestBodyStream = new MemoryStream())
            {
                using (MemoryStream responseBodyStream = new MemoryStream())
                {
                    Stream originalRequestBody = context.Request.Body;
                    Stream originalResponseBody = context.Response.Body;
                    try
                    {

                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                        requestBodyStream.Seek(0, SeekOrigin.Begin);
                        context.Request.Body = requestBodyStream;

                        string responseBody = "";

                        context.Response.Body = responseBodyStream;

                        Stopwatch watch = Stopwatch.StartNew();
                        await _next(context);
                        watch.Stop();

                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                        AddRequestLogsToLoggerFile(context, requestBodyText, responseBody);

                        responseBodyStream.Seek(0, SeekOrigin.Begin);

                        await responseBodyStream.CopyToAsync(originalResponseBody);


                    }
                    catch (Exception ex)
                    {
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        byte[] data = System.Text.Encoding.UTF8.GetBytes(new BaseApiResponse()
                        {
                            Success = false,
                            Message = ex.Message
                        }.ToString());

                        originalResponseBody.WriteAsync(data, 0, data.Length);
                        var url = context.Request.GetDisplayUrl();
                        if (!url.Contains("http://localhost") && !url.Contains("https://localhost"))
                        {
                            SendExceptionEmail(ex, context);
                        }
                        AddExceptionLogsToLoggerFile(context, ex, requestBodyText);

                        return;
                    }
                    finally
                    {
                        context.Request.Body = originalRequestBody;
                        context.Response.Body = originalResponseBody;
                    }
                }
            }

        }

        #region Send Exeption Email
        /// <summary>
        /// Send Exception Email
        /// </summary>
        public async Task<bool> SendExceptionEmail(Exception ex, HttpContext context)
        {
            TokenModel userTokenData = null;

            ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                userTokenData = _jwtAuthenticationService.GetUserTokenData(jwtToken);
            }
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

            string emailBody = string.Empty;
            string BasePath = Path.Combine(_hostingEnvironment.WebRootPath, _emailTemplates.ExceptionReport);
            string path = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            bool isSuccess = false;

            using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.ExceptionReport)))
            {
                emailBody = reader.ReadToEnd();
            }
            emailBody = emailBody.Replace("##LogoURL##", path + "/" + _config["Path:Logo"]);
            emailBody = emailBody.Replace("##DateTime##", Utility.ConvertFromUTC(DateTime.UtcNow, "India Standard Time").ToString("dd/MM/yyyy hh:mm:ss tt"));
            emailBody = emailBody.Replace("##RequestedURL##", context.Request.GetDisplayUrl());
            emailBody = emailBody.Replace("##ExceptionMessage##", ex.Message);
            emailBody = emailBody.Replace("##RequestParams##", paramValues.HeaderValue);
            emailBody = emailBody.Replace("##QueryStringParams##", paramValues.QueryStringValue);
            emailBody = ex.InnerException != null ? emailBody.Replace("##InnerException##", ex.InnerException.ToString()) : emailBody.Replace("##InnerException##", string.Empty);
            emailBody = userTokenData != null && userTokenData?.Id != null ? emailBody = emailBody.Replace("##UserId##", userTokenData.Id.ToString()) : emailBody.Replace("##UserId##", string.Empty);
            emailBody = userTokenData != null && userTokenData.FullName != null ? emailBody = emailBody.Replace("##UserName##", userTokenData.FullName.ToString()) : emailBody.Replace("##UserName##", string.Empty);

            isSuccess = await Task.Run(() => SendMailMessage(_appSettings.ExceptionEmailSend, null, null, "Exception Log Alert !", emailBody, setting, null));
            if (isSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region AddReqResLogsToLoggerFile
        /// <summary>
        /// Store ReqRes in Logger Exception file
        /// </summary>
        private void AddRequestLogsToLoggerFile(HttpContext context, String requestBodyText, String responseBody)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                TokenModel userTokenData = null;
                ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
                string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    userTokenData = _jwtAuthenticationService.GetUserTokenData(jwtToken);
                }
                string userFullName = userTokenData != null ? userTokenData.Id + " ( " + userTokenData.FullName + " )" : "";
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          Environment.NewLine + "--------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ---------------------------" +
                          Environment.NewLine + "User : " + userFullName +
                          Environment.NewLine + "Requested URL: " + context.Request.Path.Value +
                          Environment.NewLine + "Request Body: " + requestBodyText +
                          Environment.NewLine + "Query String Params: " + paramValues.QueryStringValue +
                          Environment.NewLine + "Response: " + responseBody +
                          Environment.NewLine + "Status Code: " + context.Response.StatusCode +
                          Environment.NewLine);
                logger.Info(sb.ToString());
                DeleteOldReqResLogFiles();

            }
            catch (Exception ex)
            {
                logger.Error("Exception in AddRequestLogsToLoggerFile: ", ex.Message.ToString());
            }


        }
        #endregion

        #region AddExceptionLogsToLoggerFile
        /// <summary>
        /// Store exception in Logger Exception file
        /// </summary>
        private void AddExceptionLogsToLoggerFile(HttpContext context, Exception ex, string requestBody)
        {
            TokenModel userTokenData = null;
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                userTokenData = _jwtAuthenticationService.GetUserTokenData(jwtToken);
            }
            string userFullName = userTokenData != null ? userTokenData.Id + " ( " + userTokenData.FullName + " )" : "";
            StringBuilder sb = new StringBuilder();


            sb.Append(Environment.NewLine +
                      Environment.NewLine + "--------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ---------------------------" +
                      Environment.NewLine + "User : " + userFullName +
                      Environment.NewLine + "Requested URL: " + context.Request.Path.Value +
                      Environment.NewLine + "Request Params: " + requestBody +
                      Environment.NewLine + "Query String Params: " + paramValues.QueryStringValue +
                      Environment.NewLine + "Status Code: " + context.Response.StatusCode +
                      Environment.NewLine + "Exception: " + ex.Message +
                      Environment.NewLine + "Exception: " + ex.InnerException +
                      Environment.NewLine);
            logger.Error(sb.ToString());
            DeleteOldExceptionLogFiles();
        }

        #endregion

        #region DeleteOldLogsFile
        ///// <summary>
        ///// Delete files from ReqRes logs folder which is older than 7 days.
        ///// </summary>
        public void DeleteOldReqResLogFiles()
        {
            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, Constants.LogsFilePathRequest);
            string[] Dfiles = Directory.GetFiles(directoryPath);

            foreach (string file in Dfiles)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                {
                    fi.Delete();
                }
            }
        }

        ///// <summary>
        ///// Delete files from error logs folder which is older than 7 days.
        ///// </summary>
        public void DeleteOldExceptionLogFiles()
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, Constants.LogsFilePathException);

            string[] files = Directory.GetFiles(filePath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                {
                    fi.Delete();
                }
            }
        }
        #endregion

    }
}
