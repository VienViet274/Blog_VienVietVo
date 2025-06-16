using Blog_Client.Models;
using Blog_Client.Service.IService;
using Blog_Utilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Net.Http.Headers;

namespace Blog_Client.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<APIResponse> SendAsync(APIRequest request)
        {
            var client = _httpClientFactory.CreateClient("Vo Vien Viet");
            HttpRequestMessage HttpRequestMessage = new HttpRequestMessage();
            switch (request.APIType)
            {
                case APIType.GET:
                    HttpRequestMessage.Method = HttpMethod.Get;
                    break;
                case APIType.POST:
                    HttpRequestMessage.Method = HttpMethod.Post;
                break;
                case APIType.PUT:
                    HttpRequestMessage.Method = HttpMethod.Put;
                break;
                    case APIType.DELETE:
                    HttpRequestMessage.Method = HttpMethod.Delete;
                    break;            
            }
            HttpRequestMessage.RequestUri = new Uri(request.URL);
            if (request.token != null) 
            {
                HttpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.token);
            }
            if (request.body != null) 
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(request.body),
            Encoding.UTF8, "application/json");
                HttpRequestMessage.Content = content;
            }
            APIResponse aPIResponse = new APIResponse();
            try
            {
                var result = await client.SendAsync(HttpRequestMessage);
                var resultString = await result.Content.ReadAsStringAsync();
                
                aPIResponse = JsonConvert.DeserializeObject<APIResponse>(resultString);
                return aPIResponse;
            }
            catch (Exception ex) 
            {
                aPIResponse.statusCode = HttpStatusCode.BadRequest;
                aPIResponse.errorList = new List<string> { ex.Message };
                aPIResponse.success = false;
                return aPIResponse;
            }
            
        }
    }
}
