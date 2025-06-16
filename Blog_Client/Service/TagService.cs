using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Blog_Utilities;

namespace Blog_Client.Service
{
    public class TagService: BaseService, ITagSevice
    {
        private readonly string url;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public TagService( IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            url = _configuration.GetValue<string>("ServiceUrls:blogAPI"); 
        }
        public async Task<APIResponse> Create(TagCreateDTO tag, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Tag",
                APIType = APIType.POST,
                token = token,
                body = tag
            };
            return await SendAsync(aPIRequest);
        }
        public async Task<APIResponse> Delete(int id, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Tag/{id}",
                APIType = APIType.DELETE,
                token = token
            };
            return await SendAsync(aPIRequest);
        }
        public async Task<APIResponse> GetAll(string token, string Search)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Tag",
                APIType = APIType.GET,
                token = token
            };
            if (Search != null)
            {
                aPIRequest.URL = $"{url}/Tag?Search={Search}";
            }
            return await SendAsync(aPIRequest);
        }
        public async Task<APIResponse> GetOne(int id, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Tag/{id}",
                APIType = APIType.GET,
                token = token
            };
            return await SendAsync(aPIRequest);
        }
        public async Task<APIResponse> Update(TagUpdateDTO tag, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Tag/{tag.Id}",
                APIType = APIType.PUT,
                token = token,
                body = tag
            };
            return await SendAsync(aPIRequest);
        }
    }
    
}
