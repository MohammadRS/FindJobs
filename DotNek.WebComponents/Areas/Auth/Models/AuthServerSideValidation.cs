using FindJobs.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FindJobs.Web.Helper
{
    public class AuthServerSideValidation
    {
  
        public ReCaptchaResponseModel CheckRecaptcha(string token, string secretKey)
        {
            var urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            var gRecaptchaResponse = token;

            //form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            var result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponseModel>(result);
            return captchaResponse;
        }

        public async Task<FacebookDto> ValidateFaceBookToken(string FacebookToken)
        {
            HttpClient client = new HttpClient();
            var Res=   await client.GetStringAsync("https://graph.facebook.com/me?fields=first_name,last_name,email&access_token=" + FacebookToken);
            var convertResult = JsonConvert.DeserializeObject<FacebookDto>(Res);
            return convertResult;
        }

       
    }
}
