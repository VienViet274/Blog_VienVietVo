using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog_API.Controllers
{
    [Route("BlogRating")]
    public class BlogRatingController : Controller
    {
        //private readonly ApplicationDbContext _context;
        IBlogRatingRepository _blogratingReposiory;
        IUserRepository _userRepository;
        public BlogRatingController(IBlogRatingRepository blogratingReposiory, IUserRepository userRepository)
        {
            _blogratingReposiory = blogratingReposiory;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Index()
        {
            var list = await _blogratingReposiory.GetALL();
            APIResponse aPIResponse = new APIResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                data = list
            };


            return Ok(aPIResponse);
        }
        [HttpGet("{id:int}", Name = "GetOneBlogRating")]
        public async Task<ActionResult<APIResponse>> GetOne(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "id can not be 0" };
                return BadRequest(aPIResponse);
            }
            var item = await _blogratingReposiory.GetOne(x => x.Id == id);
            if (item == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "no blog rating is found" };
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = item;
            return Ok(aPIResponse);
        }
        [HttpPost]
        public async Task<ActionResult<APIResponse>> BlogRatingCreate([FromBody] BlogRatingDTO DTO)
        {
            APIResponse aPIResponse = new APIResponse();
            if (DTO == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "input can not be null" };
                return BadRequest(aPIResponse);
            }
            var userID = _userRepository.GetUserID(User);
            BlogRating BlogRating = new BlogRating() 
            {
                BlogID = DTO.BlogID,
                Rating = DTO.Rating,
                UserID = userID
            };
            await _blogratingReposiory.Create(BlogRating);
            //await _context.SaveChangesAsync();
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = "Create blog rating successfully";
            return Ok(aPIResponse);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> BlogRatingDelete(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "id can not be 0" };
                return BadRequest(aPIResponse);
            }
            var itemDelete = await _blogratingReposiory.GetOne(x => x.Id == id);
            if (itemDelete == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "no item with id input is found" };
                return NotFound(aPIResponse);
            }
            await _blogratingReposiory.Delete(itemDelete);
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = "delete successfully";
            return Ok(aPIResponse);
        }
        //[HttpPut("{id:int}")]
        //public async Task<ActionResult<APIResponse>> CategoryUpdate(int id, [FromBody] BlogRatingDTO BlogRatingDTO)
        //{
        //    var blogratingupdate = await _blogratingReposiory.GetOne(x => x.Id == id);
        //    APIResponse aPIResponse = new APIResponse();
        //    if (blogratingupdate == null)
        //    {
        //        aPIResponse.Success = false;
        //        aPIResponse.StatusCode = HttpStatusCode.NotFound;
        //        aPIResponse.ErrorList = new List<string> { "no item with id input is found" };
        //        return NotFound(aPIResponse);
        //    }
        //    else if (categoryUpdate.Name == categoryDTO.Name)
        //    {
        //        aPIResponse.Success = false;
        //        aPIResponse.StatusCode = HttpStatusCode.BadRequest;
        //        aPIResponse.ErrorList = new List<string> { "category name existed" };
        //        return BadRequest(aPIResponse);
        //    }
        //    Category category = new Category
        //    {
        //        Id = id,
        //        Name = categoryDTO.Name
        //    };
        //    await _categoryReposiory.Update(category);
        //    var updatedItem = await _categoryReposiory.GetOne(x => x.Id == id);
        //    aPIResponse.Success = true;
        //    aPIResponse.StatusCode = HttpStatusCode.OK;
        //    aPIResponse.data = updatedItem;
        //    return Ok(aPIResponse);
        //}
    }
}
