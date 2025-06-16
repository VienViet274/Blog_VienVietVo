using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage.Repo_Interface;
using Blog_API.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog_API.Repository_Storage
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        public UserRepository(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<bool> IsUniqueUser(string username)
        {
            var userExisted = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == username);
            if (userExisted == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.Username.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = new UserDTO { Id = user.Id, Name = user.Name, Username = user.UserName },

            };
            return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegisterDTO registerationDTO)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Name = registerationDTO.Name,
                Email = registerationDTO.Email,
                UserName = registerationDTO.UserName,
                NormalizedEmail = registerationDTO.UserName.ToUpper()
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, registerationDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("reader"));
                        //await _roleManager.CreateAsync(new IdentityRole("contributor"));
                    }
                    switch (registerationDTO.RoleID)
                    {
                        case "admin":
                            await _userManager.AddToRoleAsync(applicationUser, "admin");
                            break;
                        case "contributor":
                            await _userManager.AddToRoleAsync(applicationUser, "contributor");
                            break;
                        case "reader":
                            await _userManager.AddToRoleAsync(applicationUser, "reader");
                            break;
                        default:
                            break;
                    }
                    var userToReturn = _context.ApplicationUsers
                        .FirstOrDefault(u => u.UserName == registerationDTO.UserName);
                    return new UserDTO
                    {
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        Username = userToReturn.UserName
                    };
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public string GetUserID(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
        }
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<List<UserWithRolesDTO>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userWithRolesList = new List<UserWithRolesDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRolesList.Add(new UserWithRolesDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return userWithRolesList;
        }

        public async Task UpdateUserRole(UserWithRolesDTO userWithRolesDTO)
        {
            var user = await _userManager.FindByIdAsync(userWithRolesDTO.Id);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, userWithRolesDTO.Roles);
            }
        }

        public async Task<UserWithRolesDTO> GetOneUsersWithRoles(string UserID)
        {
            var list = await GetAllUsersWithRoles();
            var user = list.FirstOrDefault(x => x.Id == UserID);
            if (user != null)
            {
                return user;
            }
            else
            {
                return new UserWithRolesDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    Roles = user.Roles
                };
            }
        }
    }
}
