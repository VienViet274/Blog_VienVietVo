using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata;

namespace Blog_API.Controllers
{
    [Route("Tag")]
    public class TagController : Controller
    {
        //private readonly ApplicationDbContext _context;
        ITagRepository _tagReposiory;
        IBlogRepository _blogRepository;
        public TagController(ITagRepository tagReposiory, IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _tagReposiory = tagReposiory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            APIResponse aPIResponse = new APIResponse();
            var list = await _tagReposiory.GetALL();
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.Success = true;
            aPIResponse.data = list;
            return Ok(aPIResponse);
        }
        [HttpGet("{id:int}", Name = "GetOneTag")]
        public async Task<IActionResult> GetOne(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.StatusCode=HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "id must not be 0"};
                aPIResponse.Success = false;
                return BadRequest();
            }
            var item = await _tagReposiory.GetOne(x => x.Id == id,"Blogs");
            if (item == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id = {id}" };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = item;
            return Ok(aPIResponse);
        }
        [HttpPost]
        public async Task<IActionResult> TagCreate([FromBody] TagDTO DTO)
        {
            APIResponse aPIResponse = new APIResponse();
            if (DTO == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "Please input data" };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(aPIResponse);
            }
            Tag tag = new Tag() { Name = DTO.Name };
            var validBlogIDs = DTO.BlogIDs.Where(x=>x>0).Distinct().ToList();
            if (validBlogIDs.Count > 0) 
            {
                tag.Blogs = new List<Blog>();
                var blogs = await _blogRepository.GetALL(x => validBlogIDs.Contains(x.Id));
                tag.Blogs.AddRange(blogs);
            }
            await _tagReposiory.Create(tag);
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.Success = true;
            aPIResponse.data = new { message = "created tag successfully", data = tag };
            return Ok(aPIResponse);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> TagDelete(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { "id must not be 0" };
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }
            var itemDelete = await _tagReposiory.GetOne(x => x.Id == id,"Blogs");
            if (itemDelete == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id ={id} " };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }
            await _tagReposiory.Delete(itemDelete);
            aPIResponse.Success = true;
            aPIResponse.data = "Delete successfully";
            aPIResponse.StatusCode = HttpStatusCode.OK;
            return Ok(aPIResponse);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> TagUpdate(int id, [FromBody] TagDTO tagDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            var tagUpdate = await _tagReposiory.GetOne(x => x.Id == id,"Blogs",false);
            if (tagUpdate == null)
            {
                aPIResponse.Success = false;
                aPIResponse.ErrorList = new List<string> { $"no item is found with id ={id} " };
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound("No category found");
            }
            tagUpdate.Name = tagDTO.Name;
            var validBlogIDs = tagDTO.BlogIDs.Where(x => x > 0).Distinct().ToList();
            if (validBlogIDs.Count > 0)
            {
                var blogs = await _blogRepository.GetALL(x => validBlogIDs.Contains(x.Id));
                //tag.Blogs.AddRange(blogs);
                tagUpdate.Blogs.Clear();
                foreach (var blog in blogs) 
                {
                    tagUpdate.Blogs.Add(blog);
                }
            }
            await _tagReposiory.Update(tagUpdate);
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.Success = true;
            aPIResponse.data = new { message = "updated tag successfully", data = tagUpdate };
            return Ok(aPIResponse);
        }
    }
}
