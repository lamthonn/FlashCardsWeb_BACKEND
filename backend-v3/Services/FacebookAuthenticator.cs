using System;
using System.Net.Http;
using System.Threading.Tasks;
using backend_v3.Dto;
using backend_v3.Interfaces;
using Newtonsoft.Json.Linq;

namespace backend_v3.Services
{
    public class FacebookAuthenticator : IFacebookAuthenticator
    {
        private readonly HttpClient _httpClient;

        public FacebookAuthenticator()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> ValidateAccessTokenAsync(FacebookRequest request)
        {
            var appSecret = "1e2800287e72613e79bd27b8bdba81d2";
            var appId = "959774698897995";
            // Gửi yêu cầu HTTP đến Facebook để kiểm tra token
            var response = await _httpClient.GetAsync($"https://graph.facebook.com/debug_token?input_token={request.AccessToken}&access_token={appId}|{appSecret}");

            // Đảm bảo yêu cầu thành công
            if (response.IsSuccessStatusCode)
            {
                // Đọc nội dung phản hồi
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseContent);

                // Kiểm tra xem token có hợp lệ hay không
                return (bool)json["data"]["is_valid"];
            }
            else
            {
                // Xử lý lỗi nếu có
                throw new HttpRequestException($"Facebook validation failed with status code {(int)response.StatusCode}");
            }
        }
    }
}
