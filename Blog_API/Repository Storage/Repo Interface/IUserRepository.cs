using Blog_API.Models;
using Blog_API.Models.DTO;
using System.Security.Claims;

namespace Blog_API.Repository_Storage.Repo_Interface
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterDTO registerationRequestDTO);
        string GetUserID(ClaimsPrincipal user);
        public Task UpdateUserRole(UserWithRolesDTO userWithRolesDTO);
        public Task<ApplicationUser> GetUserById(string userId);
        public Task<List<UserWithRolesDTO>> GetAllUsersWithRoles();
        public Task<UserWithRolesDTO> GetOneUsersWithRoles(string UserID);
    }
}
