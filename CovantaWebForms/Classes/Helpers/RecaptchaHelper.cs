using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;

namespace CovantaWebForms.Classes.Helpers
{
    public class RecaptchaHelper
    {
        /// <summary>
        /// Check if ReCaptcha is Valid.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static bool IsReCaptchValid(HttpRequest Request)
        {
            var result = false;
            try
            {
                bool recaptchaValidationEnabled = bool.Parse(ConfigurationManager.AppSettings["EnableRecaptchaValidation"]);

                //Check if ReCaptcha is enabled in web.config.
                if (recaptchaValidationEnabled)
                {
                    var captchaResponse = Request.Form["g-recaptcha-response"];
                    var secretKey = ConfigurationManager.AppSettings["reCaptcha-SecretKey"];
                    var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
                    var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
                    var request = (HttpWebRequest)WebRequest.Create(requestUri);

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                        {
                            JObject jResponse = JObject.Parse(stream.ReadToEnd());
                            var isSuccess = jResponse.Value<bool>("success");
                            result = (isSuccess) ? true : false;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}