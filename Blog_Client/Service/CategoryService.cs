using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Blog_Utilities;

namespace Blog_Client.Service
{
    public class CategoryService: BaseService, ICategoryService
    {
        private readonly string url;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryService( IConfiguration configuration, IHttpClientFactory httpClientFactory): base(httpClientFactory)
        {
            
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
           url = _configuration.GetValue<string>("ServiceUrls:blogAPI");
        }

        public async Task<APIResponse> Create(CategoryCreateDTO category, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Category",
                APIType = APIType.POST,
                token = token,
                body = category
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> Delete(int id, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Category/{id}",
                APIType = APIType.DELETE,
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetAll(string token, string Search)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Category",
                APIType = APIType.GET,
                token = token
            };
            if (Search!=null)
            {
                aPIRequest.URL = $"{url}/Category?Search={Search}";
            }
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetOne(int id, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Category/{id}",
                APIType = APIType.GET,
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> Update(CategoryUpdateDTO category, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Category/{category.Id}",
                APIType = APIType.PUT,
                token = token,
                body = category
            };
            return  await SendAsync(aPIRequest);
        }
    }
}
