using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Models.DTO;
using Blog_API.Repository_Storage.Repo_Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog_API.Controllers
{
    [Route("Category")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;
        ICategoryRepository _categoryReposiory;
        public CategoryController(ICategoryRepository categoryReposiory)
        {
            _categoryReposiory = categoryReposiory;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Index()
        {
            var list = await _categoryReposiory.GetALL();
            APIResponse aPIResponse = new APIResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                data = list
            };
            

            return Ok(aPIResponse);
        }
        [HttpGet("{id:int}" , Name = "GetOneCategory")]
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
            var item = await _categoryReposiory.GetOne(x => x.Id == id);
            if (item == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "no category is found" };
                return NotFound(aPIResponse);
            }
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = item;
            return Ok(aPIResponse);
        }
        [HttpPost]
        public async Task<ActionResult<APIResponse>> CategoryCreate([FromBody] CategoryDTO DTO)
        {
            APIResponse aPIResponse = new APIResponse();
            if (DTO == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "input can not be null" };
                return BadRequest(aPIResponse);
            }
            Category category = new Category() { Name = DTO.Name };
            await _categoryReposiory.Create(category);
            //await _context.SaveChangesAsync();
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = "Create category successfully";
            return Ok(aPIResponse);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> CategoryDelete(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            if (id == 0 || id == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "id can not be 0" };
                return BadRequest(aPIResponse);
            }
            var itemDelete =await _categoryReposiory.GetOne(x=>x.Id== id);
             if (itemDelete == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "no item with id input is found" };
                return NotFound(aPIResponse);
            }
            await _categoryReposiory.Delete(itemDelete);
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = "delete successfully";
            return Ok(aPIResponse);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> CategoryUpdate(int id,[FromBody]CategoryDTO categoryDTO)
        {
            var categoryUpdate = await _categoryReposiory.GetOne(x=>x.Id==id);
            APIResponse aPIResponse = new APIResponse();
            if (categoryUpdate == null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.NotFound;
                aPIResponse.ErrorList = new List<string> { "no item with id input is found" };
                return NotFound(aPIResponse);
            }
           var checkExisted =  await _categoryReposiory.GetOne(x => x.Name == categoryDTO.Name && x.Id != id);
             if (checkExisted !=null)
            {
                aPIResponse.Success = false;
                aPIResponse.StatusCode = HttpStatusCode.BadRequest;
                aPIResponse.ErrorList = new List<string> { "category name existed" };
                return BadRequest(aPIResponse);
            }
            Category category = new Category
            {
                Id = id,
                Name = categoryDTO.Name
            };
            await _categoryReposiory.Update(category);
            var updatedItem = await _categoryReposiory.GetOne(x=>x.Id == id);
            aPIResponse.Success = true;
            aPIResponse.StatusCode = HttpStatusCode.OK;
            aPIResponse.data = updatedItem;
            return Ok(aPIResponse);
        }
    }
}
