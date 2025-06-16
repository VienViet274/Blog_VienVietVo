using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Blog_Utilities;

namespace Blog_Client.Service
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string url;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            url = _configuration.GetValue<string>("ServiceUrls:blogAPI");
        }

        public async Task<APIResponse> GetAllUser(string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.GET,
                URL = $"{url}/Auth/GetAllUser",
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetApplicationUser(string userID, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.GET,
                URL = $"{url}/Auth/GetuserById/{userID}",
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> GetOneUser(string userID, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.GET,
                URL = $"{url}/Auth/GetUserWithRoles/{userID}",
                token = token
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> Login(string username, string password)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.POST,
                URL = $"{url}/Auth/Login",
                body = new { Username = username, Password = password }
            };
            return await SendAsync(aPIRequest);
        }

        public async Task<APIResponse> Register(RegisterDTO registerDTO)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.POST,
                URL = $"{url}/Auth/Register",
                body = registerDTO
            };
           return await  SendAsync(aPIRequest);
        }

        public async Task<APIResponse> UpdateUserRoles(UserWithRolesDTO userWithRolesDTO, string token)
        {
            APIRequest aPIRequest = new APIRequest
            {
                APIType = APIType.POST,
                URL = $"{url}/Auth/UpdateUserRoles",
                body = userWithRolesDTO,
                token = token
            };
            return await SendAsync(aPIRequest);
        }
    }
}
