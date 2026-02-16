using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace User.Application.Helper
{
    public class ApiHelper
    {
        private readonly HttpContext _httpContext;

        public ApiHelper(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public HttpResponseMessage ApiCall(string apiUrl, string endPoint, string Method, object? Parameter = null)
        {

            var request = _httpContext.Request.Headers["device_id"];
            var authHeader = _httpContext.Request.Headers["Authorization"];
            string Token = string.Empty;
            string deviceId = string.Empty;

            HttpResponseMessage response = new HttpResponseMessage();
            var url = apiUrl + string.Format(endPoint);

            HttpClient client = new HttpClient();

            if (authHeader.Count != 0)
            {
                Token = authHeader[0].Substring("Bearer ".Length).Trim();
                if (!string.IsNullOrEmpty(Token))
                {
                    client.SetBearerToken(Token);
                }
            }

            if (request.Count > 0)
            {
                deviceId = request[0].ToString();
                if (!string.IsNullOrEmpty(deviceId))
                {
                    client.DefaultRequestHeaders.Add("device_id", deviceId);
                }
            }

            try
            {

                if (Method.ToLower() == "get")
                {
                    response = client.GetAsync(url).Result;
                }
                else if (Method.ToLower() == "post")
                {
                    var json = JsonConvert.SerializeObject(Parameter);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    response = client.PostAsync(url, data).Result;
                }
                else if (Method.ToLower() == "put")
                {
                    var json = JsonConvert.SerializeObject(Parameter);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    response = client.PutAsync(url, data).Result;
                }
                else if (Method.ToLower() == "delete")
                {
                    response = client.DeleteAsync(url).Result;
                }

                //if (!response.IsSuccessStatusCode)
                //{
                //    //throw new Exception("API failed with HttpStatus" + response.StatusCode);
                //}


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }



            return response;
        }
    }
}
