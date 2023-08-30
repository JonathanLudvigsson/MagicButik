using Newtonsoft.Json;
using System.Text;
using Web_Coupon.Models;

namespace Web_Coupon.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }

        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            responseModel = new ResponseDto();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("SUT22CouponAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,"application/json");
                }

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        break;
                }

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch(Exception e)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };

                var response = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(response);
                return apiResponseDto;
            }
        }
    }
}
