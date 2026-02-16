using System.Net.Http.Headers;

namespace API_Gateway.Helper
{
    public static class TokenRelay
    {

        public static string GetBearerToken(IHttpContextAccessor httpContextAccessor)
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (authorizationHeader.Count > 0)
            {
                return authorizationHeader[0].Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        public static HttpClient CreateClient(string baseUrl, string token)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

    }
}
