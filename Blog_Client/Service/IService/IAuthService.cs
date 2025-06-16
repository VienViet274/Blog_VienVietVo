using Blog_Client.Models;
using Blog_Client.Models.DTO;

namespace Blog_Client.Service.IService
{
    public interface IAuthService
    {
        Task<APIResponse> Register(RegisterDTO registerDTO);
        Task<APIResponse> Login(string username, string password);
        Task<APIResponse> GetAllUser(string token);
        Task<APIResponse> GetOneUser(string userID, string token);
        Task<APIResponse> UpdateUserRoles(UserWithRolesDTO userWithRolesDTO, string token);
        Task<APIResponse> GetApplicationUser(string userID, string token);
    }
}
