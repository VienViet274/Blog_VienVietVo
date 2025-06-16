using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Blog_Utilities;

namespace Blog_Client.Service
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string url;
        private readonly IConfiguration _configuration;
        public CommentService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            url = _configuration.GetValue<string>("ServiceUrls:blogAPI");
        }
        public async Task<APIResponse> AddComment(CommentDTO commentDTO, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.POST,
                URL = $"{url}/Comment",
                body = commentDTO,
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> ApproveComment(int commentId, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.GET,
                URL = $"{url}/Comment/ApproveComment/{commentId}",
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetAllComments(int blogID, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.GET,
                URL = $"{url}/Comment",
                token = token
            };
            return await SendAsync(aPIRequest);
        }
        public async Task<APIResponse> GetOneComment(int blogID, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.GET,
                URL = $"{url}/Comment/GetComment/{blogID}",
                token = token
            };
            return await SendAsync(aPIRequest);
        }

       
    }
}
