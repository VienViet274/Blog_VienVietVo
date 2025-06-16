using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Blog_Utilities;

namespace Blog_Client.Service
{
    public class BlogService : BaseService, IBlogService
    {
        private readonly string url;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public BlogService(IConfiguration configuration, IHttpClientFactory httpClientFactory): base(httpClientFactory)
        {
            _configuration = configuration;
            url = _configuration.GetValue<string>("ServiceUrls:blogAPI");
            _httpClientFactory = httpClientFactory;
        }

        public async Task<APIResponse> Create(BlogCreateDTO blogCreateDTO, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Blog",
                APIType = APIType.POST,
                token = token,
                body = blogCreateDTO
                //new
                //{
                //    DTO = formdata,
                //    file = file!= null ? new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, file.FileName) : null
                //}
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> Delete(int id, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Blog/{id}",
                APIType = APIType.DELETE,
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetAll(string token, string Search)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Blog",
                APIType = APIType.GET,
                token = token
            };
            if (Search != null) 
            {
                aPIRequest.URL = $"{url}/Blog?Search={Search}";
            }
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetOne(int id, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Blog/{id}",
                APIType = APIType.GET,
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> Update(BlogUpdateDTO blog, string token)
        {
            
            APIRequest aPIRequest = new APIRequest
            {
                URL = $"{url}/Blog/{blog.Id}",
                APIType = APIType.PUT,
                token = token,
                body = blog
            };
            return await SendAsync(aPIRequest);
        }
    }
}
