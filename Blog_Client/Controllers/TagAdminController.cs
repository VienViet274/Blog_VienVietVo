using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog_Client.Controllers
{
    public class TagAdminController : Controller
    {
        private readonly ITagSevice _tagService;
        private readonly IBlogService _blogService;
        public TagAdminController(ITagSevice tagSevice, IBlogService blogService)
        {
            _tagService = tagSevice;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _tagService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            if (response != null && response.success)
            {
                string stringTag = response.data.ToString();
                var tags = JsonConvert.DeserializeObject<List<Tag>>(stringTag);
                return View(tags);
            }
            ViewBag.message = response.errorList.FirstOrDefault() ?? "Khong co du lieu";
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            var response = await _tagService.GetOne(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.success)
            {
                string stringTag = response.data.ToString();
                var tag = JsonConvert.DeserializeObject<Tag>(stringTag);
                var blogsResponse = await _blogService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
                if (blogsResponse != null && blogsResponse.success)
                {
                    string stringBlogs = blogsResponse.data.ToString();
                    var blogs = JsonConvert.DeserializeObject<List<Blog>>(stringBlogs);
                    ViewBag.Blogs = blogs;
                }
                else
                {
                    ViewBag.message = blogsResponse.errorList.FirstOrDefault() ?? "Khong co du lieu blog";
                }
                TagUpdateDTO tagUpdateDTO = new TagUpdateDTO
                {
                    Id = tag.id,
                    Name = tag.name,
                    BlogIDs = tag.blogs.Select(b => b.id).ToList()
                };
                return View(tagUpdateDTO);
            }
            ViewBag.message = response.errorList.FirstOrDefault() ?? "Khong co du lieu";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(TagUpdateDTO tagUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _tagService.Update(tagUpdateDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.message = response.errorList.FirstOrDefault() ?? "Khong co du lieu";
            }
            return View(tagUpdateDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _tagService.Delete(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.success)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.message = response.errorList.FirstOrDefault() ?? "Khong co du lieu";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create()
        {
            var blogsResponse = await _blogService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            if (blogsResponse != null && blogsResponse.success)
            {
                string stringBlogs = blogsResponse.data.ToString();
                var blogs = JsonConvert.DeserializeObject<List<Blog>>(stringBlogs);
                ViewBag.Blogs = blogs;
            }
            else
            {
                ViewBag.message = blogsResponse.errorList.FirstOrDefault() ?? "Khong co du lieu blog";
            }
            return View(new TagCreateDTO());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateDTO tagCreateDTO)
        {
            if (ModelState.IsValid)
            {
                if (tagCreateDTO.BlogIDs == null)
                {
                tagCreateDTO.BlogIDs = new List<int>();
                }
                var response = await _tagService.Create(tagCreateDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.message = response.errorList.FirstOrDefault() ?? "Khong co du lieu";
            }
            var blogsResponse = await _blogService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            string stringBlogs = blogsResponse.data.ToString();
            var blogs = JsonConvert.DeserializeObject<List<Blog>>(stringBlogs);
            ViewBag.Blogs = blogs;
            return View(tagCreateDTO);
        }
    }
}
