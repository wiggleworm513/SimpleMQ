using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Web.Util
{
    public class HttpClientHelper
    {
        public static void DeleteServiceAsync(string path)
        {
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(200.00);
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, SecurityHelper.GetAuthToken());
                HttpResponseMessage response = httpClient.DeleteAsync(path).GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();
            }
        }

        public static string GetServiceAsync(string path)
        {
            string data = string.Empty;

            using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(200.00);
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, SecurityHelper.GetAuthToken());
                HttpResponseMessage response = httpClient.GetAsync(path).GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            return data;
        }

        public static string PostServiceWithResultsAsync(string path, string content)
        {
            string data = string.Empty;

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(200.00);
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, SecurityHelper.GetAuthToken());
                HttpResponseMessage response = httpClient.PostAsync(path, byteContent).GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            return data;
        }
    }
}
