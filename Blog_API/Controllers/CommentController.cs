using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog_API.Controllers
{
    [Route("Comment")]
    [Authorize]
    public class CommentController : Controller
    {
        ICommentRepository _commentRepository;
        IUserRepository _userRepository;
        string userID;

        public CommentController(ICommentRepository commentRepository,IUserRepository userRepository)

        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
           //userID =  _userRepository.GetUserID(User);
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            //var test = _userRepository.GetUserID(User);
            //var test = User;
            APIResponse aPIResponse = new APIResponse();
            var list = await _commentRepository.GetALL();
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.Success = true;
            aPIResponse.data = list;
            return Ok(aPIResponse);
        }
        //[Route("GetComment")]
        [HttpGet("GetComment/{blogID:int}", Name = "GetOneComment")]
        public async Task<ActionResult<APIResponse>> GetOne(int blogID)
        {
            APIResponse aPIResponse = new APIResponse();
            if (blogID == 0 || blogID == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "blogID must not be 0" };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            var item = await _commentRepository.GetALL(x => x.BlogID == blogID);
            if (item == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id = {blogID}" };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = item;
            return Ok(aPIResponse);
        }
        [HttpPost]
        public async Task<ActionResult<APIResponse>> CommentCreate([FromBody] CommentDTO DTO)
        {
            APIResponse aPIResponse = new APIResponse();
            if (DTO == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "Please input data" };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            userID = _userRepository.GetUserID(User);
            Comment comment = new Comment()
            {
                Message = DTO.message,
                BlogID = DTO.BlogID,
                UserID = userID,
                CreatedAt = DateTime.UtcNow,
            };
            try
            {
                await _commentRepository.Create(comment);
                aPIResponse.StatusCode = HttpStatusCode.OK;
                aPIResponse.Success = true;
                aPIResponse.data = new { message = "created comment successfully", data = comment };
                return Ok(aPIResponse);
            }
            catch (Exception ex) 
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { ex.Message };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            //await _context.SaveChangesAsync();
            
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> CommentDelete(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "id must not be 0" };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }
            var itemDelete = await _commentRepository.GetOne(x => x.Id == id);
            if (itemDelete == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id ={id} " };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }
            await _commentRepository.Delete(itemDelete);
            aPIResponse.Success = true;
            aPIResponse.data = "Delete successfully";
            aPIResponse.StatusCode = HttpStatusCode.OK;
            return Ok(aPIResponse);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> CommentUpdate(int id, [FromBody] CommentDTO CommentDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            var blogUpdate = await _commentRepository.GetOne(x => x.Id == id);
            if (blogUpdate == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id ={id} " };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(aPIResponse);
            }
            userID = _userRepository.GetUserID(User);
            Comment Comment = new Comment
            {
                Id = id,
                Message = CommentDTO.message,
                BlogID = CommentDTO.BlogID,
                UserID = userID,
                CreatedAt = blogUpdate.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
            };
           
            await _commentRepository.Update(Comment);
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.Success = true;
            aPIResponse.data = Comment;
            return Ok(aPIResponse);
        }
        //[Route("ApproveComment")]
        [HttpGet("ApproveComment/{id:int}", Name = "ApproveComment")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> ApproveComment(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            var commentFound = await _commentRepository.GetOne(x => x.Id == id);
            if(commentFound == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "id is not found" };
                aPIResponse.StatusCode= HttpStatusCode.NotFound;
                return NotFound(aPIResponse);
            }
            commentFound.Approved = true;
            await _commentRepository.Update(commentFound);
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data= commentFound;
            return Ok(aPIResponse);
        }
    }
}
