using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace Blog_API.Controllers
{
    [Route("Blog")]
    //[Authorize(Roles ="admin,contributor")]
    public class BlogController : Controller
    {

        IBlogRepository _blogReposiory;
        ITagRepository _tagRepository;
        IUserRepository _userRepository;
        public BlogController(IBlogRepository blogReposiory, 
            ITagRepository tagRepository,
            IUserRepository userRepository)
        {
            _tagRepository = tagRepository;
            _blogReposiory = blogReposiory;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Index([FromQuery] string Search)
        {
            //var test = _userRepository.GetUserID(User);
            //var test = User;
             APIResponse aPIResponse = new APIResponse();
            List<Blog> list = new List<Blog>();
            if (Search == null)
            {
                list = await _blogReposiory.GetALL(null, "Category,Tags");
            }
            else
            {
                list =  await _blogReposiory.GetALL(x=>x.Title.Contains(Search), "Category,Tags");
            }
            if (list.Count !=0)
            {
                aPIResponse.StatusCode = HttpStatusCode.OK;
                aPIResponse.Success = true;
                aPIResponse.data = list;
           //     var result = await db.Database
           //.SqlQueryRaw<BlogTagRow>("SELECT * FROM BlogTags")
           //.ToListAsync();
           //     foreach (var item in result)
           //     {
           //         Console.WriteLine($"Blog ID: {item.BlogId}, Tag ID: {item.TagId}");
           //     }
                return Ok(aPIResponse);
            }
            aPIResponse.StatusCode = HttpStatusCode.NotFound;
            aPIResponse.Success = false;
            aPIResponse.data = null;
            return BadRequest(aPIResponse);
        }
        [HttpGet("{id:int}", Name = "GetOneBlog")]
        public async Task<ActionResult<APIResponse>> GetOne(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success=false;
                aPIResponse.ErrorList = new List<string> { "id must not be 0"};
                aPIResponse.StatusCode=HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            var item = await _blogReposiory.GetOne(x => x.Id == id, "Category,Tags,Comments");
            if (item == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id = {id}" };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(aPIResponse);
            }
            await Task.WhenAll(item.Comments.Select(async x => x.ApplicationUser = await _userRepository.GetUserById(x.UserID)));
            //foreach (var comment in item.Comments)
            //{
            //    comment.ApplicationUser = await _userRepository.GetUserById(comment.UserID);
            //}
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = item;
            return Ok(aPIResponse);
        }
        [HttpPost]
        [Authorize(Roles = "admin,contributor")]
        public async Task<ActionResult<APIResponse>> BlogCreate([FromBody] BlogCreateDTO DTO)
        {
            APIResponse aPIResponse = new APIResponse();
            if (!ModelState.IsValid)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList=new List<string> { "Please input data"};
                aPIResponse.StatusCode=HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            Blog blog = new Blog() { Title = DTO.Title,
                                         Summary = DTO.Summary,
                                        Content = DTO.Content,
                                        CategoryId = DTO.CategoryId,
                                       ImagePath = DTO.ImagePath,
                                    };
            var validTagIDs = DTO.TagIDs.Where(x=>x>0).Distinct().ToList();
            if (validTagIDs.Count > 0) 
            {
                var Tags = await _tagRepository.GetALL(x => validTagIDs.Contains(x.Id));
                blog.Tags = Tags;
            }
            await _blogReposiory.Create(blog);
            aPIResponse.StatusCode =HttpStatusCode.OK;
            aPIResponse.Success=true;
            aPIResponse.data = new { message = "created blog successfully", data = blog};
            //await _context.SaveChangesAsync();
            return Ok(aPIResponse);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin,contributor")]
        public async Task<ActionResult<APIResponse>> BlogDelete(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "id must not be 0" };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }
            var itemDelete = await _blogReposiory.GetOne(x => x.Id == id);
            if (itemDelete == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id ={id} " };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }
            await _blogReposiory.Delete(itemDelete);
            aPIResponse.Success = true;
            aPIResponse.data = "Delete successfully";
            aPIResponse.StatusCode = HttpStatusCode.OK;
            return Ok(aPIResponse);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin,contributor")]
        public async Task<ActionResult<APIResponse>> BlogUpdate(int id, [FromBody] BlogDTO blogDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            var blogUpdate = await _blogReposiory.GetOne(x => x.Id == id, "Tags", false);
            if (blogUpdate == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id ={id} " };
                aPIResponse.StatusCode= HttpStatusCode.NotFound;
                return NotFound(aPIResponse);
            }
            blogUpdate.Title = blogDTO.Title;
            blogUpdate.CategoryId = blogDTO.CategoryId;
            blogUpdate.Content = blogDTO.Content;
            blogUpdate.ImagePath = blogDTO.ImagePath;
            blogUpdate.Summary = blogDTO.Summary;
            blogUpdate.Tags.Clear();
            var validTagIDs = blogDTO.TagIDs.Where(x => x > 0).Distinct().ToList();
            if (validTagIDs.Count > 0)
            {
                var Tags = await _tagRepository.GetALL(x => validTagIDs.Contains(x.Id));
                foreach (var tag in Tags)
                {
                    blogUpdate.Tags.Add(tag);
                }
            }
            await _blogReposiory.Update(blogUpdate);
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.Success = true;
            aPIResponse.data = blogUpdate;
            return Ok(aPIResponse);
        }
    }
}
