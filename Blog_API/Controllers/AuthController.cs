using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog_API.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterDTO registerDTO)
        {
            APIResponse apiResponse = new APIResponse();
            bool check = await _userRepository.IsUniqueUser(registerDTO.UserName);
            if (!check)
            {
                apiResponse.ErrorList = new List<string> { "your username is existed" };
                apiResponse.Success = false;
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(apiResponse);
            }
            var userDTO = await _userRepository.Register(registerDTO);
            if (userDTO == null)
            {
                apiResponse.Success = false;
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.ErrorList = new List<string> { "error when register new user" };
                apiResponse.data = null;
                return Ok(apiResponse);
            }
            apiResponse.Success = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.data = userDTO;
            return Ok(apiResponse);
        }
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            var result = await _userRepository.Login(loginRequestDTO);
            if (result.User == null)
            {
                aPIResponse.ErrorList = new List<string> { "Login fail" };
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.data = result;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            return Ok(aPIResponse);
        }
        [Route("UpdateUserRoles")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateUserRoles([FromBody] UserWithRolesDTO UserWithRolesDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            try
            {
                await _userRepository.UpdateUserRole(UserWithRolesDTO);
                aPIResponse.Success = true;
                aPIResponse.StatusCode = HttpStatusCode.OK;
                aPIResponse.data = UserWithRolesDTO;
                return Ok(aPIResponse);
            }
            catch (Exception ex) 
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "Error when update user roles" };
                return BadRequest(aPIResponse);
            }

        }
        [Route("GetAllUser")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetAllUser()
        {
            APIResponse aPIResponse = new APIResponse();
            var list = await _userRepository.GetAllUsersWithRoles();
            if (list.Count == 0)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                aPIResponse.ErrorList = new List<string> { "No user is found" };
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = list;
            return Ok(aPIResponse);
        }
        //[Route("GetuserById")]
        [HttpGet("GetuserById/{userId:Guid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetUserById(string userId)
        {
            APIResponse aPIResponse = new APIResponse();
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                aPIResponse.ErrorList = new List<string> { "No user is found" };
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = user;
            return Ok(aPIResponse);
        }
        //[Route("getUserWithRoles")]
        [HttpGet("GetUserWithRoles/{userId:Guid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetUserWithRoles(string userId)
        {
            APIResponse aPIResponse = new APIResponse();
            var user = await _userRepository.GetOneUsersWithRoles(userId);
            if (user == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                aPIResponse.ErrorList = new List<string> { "No user is found" };
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = user;
            return Ok(aPIResponse);
        }
    }
    }
