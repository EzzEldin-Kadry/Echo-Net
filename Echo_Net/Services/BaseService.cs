using System.Text;
using Echo_Net.Models;
using Echo_Net.Models.Dto;
using Echo_Net.Services.IServices;
using Newtonsoft.Json;
namespace Echo_Net.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto ResponseDto { get; set; }
        public IHttpClientFactory HttpClient { get; set; }
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            ResponseDto = new ResponseDto();
            HttpClient = httpClientFactory;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("EchoNetAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if(apiRequest.Data is not null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                     Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case Constants.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Constants.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Constants.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}