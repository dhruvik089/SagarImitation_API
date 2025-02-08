using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using SagarImitation.Model.ReqResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SagarImitation.Common.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SagarImitation.Common.CommonMethod
{
    public class CommonMethods
    {  

        #region GetKeyValues
        /// <summary>
        /// Get key value pair result
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ParamValue GetKeyValues(HttpContext context)
        {
            ParamValue paramValues = new ParamValue();
            var headerValue = string.Empty;
            var queryString = string.Empty;
            var jsonString = string.Empty;
            StringValues outValue = string.Empty;

            // for from header value
            if (context.Request.Headers.TryGetValue(Constants.RequestModel, out outValue))
            {
                headerValue = outValue.FirstOrDefault();
                JObject jsonobj = JsonConvert.DeserializeObject<JObject>(headerValue);
                if (jsonobj != null)
                {
                    Dictionary<string, string> keyValueMap = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, JToken> keyValuePair in jsonobj)
                    {
                        keyValueMap.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                    }
                    List<ReqResponseKeyValue> keyValueMapNew = keyValueMap.ToList().Select(i => new ReqResponseKeyValue
                    {
                        Key = i.Key,
                        Value = i.Value
                    }).ToList();
                    jsonString = JsonConvert.SerializeObject(keyValueMapNew);
                }
            }
            // for from query value
            if (context.Request.QueryString.HasValue)
            {
                var dict = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
                queryString = System.Text.Json.JsonSerializer.Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k]));
            }


            paramValues.HeaderValue = jsonString;
            paramValues.QueryStringValue = queryString;
            return paramValues;

        }
        #endregion    

        public static string GenerateRandomNumber(int length)
        {
            Random random = new Random();
            char[] chars = new char[length];
            string allowedChars = "0123456789";
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        public static bool IsValidEmail(string email)
        {
            const string pattern = @"^(?=.{1,64}@.{1,255}$)([a-zA-Z0-9%+-](?:[a-zA-Z0-9._%+-]{0,62}[a-zA-Z0-9%+-])?)@([a-zA-Z0-9.-]+\.[a-zA-Z]{2,63})$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsPasswordStrong(string CreatePassword)
        {
            const string pattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])[a-zA-Z0-9\W_]{6,}$";
            return Regex.IsMatch(CreatePassword, pattern);
        }

        public static async Task<string> UploadImage(IFormFile File, string ImagePath)
        {
            Guid guidFile = Guid.NewGuid();
            string FileName;
            string BasePath;
            string path;
            string Photo = string.Empty;
            FileName = guidFile + Path.GetExtension(File.FileName);
            BasePath = Path.Combine(Directory.GetCurrentDirectory(), ImagePath);
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            path = Path.Combine(BasePath, FileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }
            return FileName;
        }     
    }
}
