using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Blog_Client.Controllers
{
    //[Authorize(Roles ="admin")]
    public class BlogAdminController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly ITagSevice _tagService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogAdminController(IBlogService blogService, ICategoryService categoryService, ITagSevice tagSevice, IWebHostEnvironment webHostEnvironment)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _tagService = tagSevice;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index([FromQuery] string Search)
        {
            var tets = await _blogService.GetAll(HttpContext.Session.GetString(SD.SessionToken), Search);
            var blogs = tets.data;
            string content = blogs.ToString();
            List<Blog> list = new List<Blog>();
            list = JsonConvert.DeserializeObject<List<Blog>>(content);
            return View(list);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id)
        {
            var tets = await _blogService.GetOne(id, HttpContext.Session.GetString(SD.SessionToken));
            var blog = tets.data;
            string blogString = blog.ToString();
            var item = JsonConvert.DeserializeObject<Blog>(blogString);
            BlogUpdateDTO blogUpdateDTO = new BlogUpdateDTO
            {
                Id = item.id,
                Title = item.title,
                Content = item.content,
                CategoryId = item.categoryId,
                ImagePath = item.imagePath,
                Summary = item.summary,
                TagIDs = item.tags.Select(tag => tag.id).ToList()
            };
            var categoryResponse = await _categoryService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            string categoryContent = categoryResponse.data.ToString();
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoryContent);
            ViewBag.Categories = categories;
            var tagResponse = await _tagService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            string tagContent = tagResponse.data.ToString();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(tagContent);
            ViewBag.AllTags = tags;
            return View(blogUpdateDTO);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Update(BlogUpdateDTO blogDTO, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var resultItem = await _blogService.GetOne(blogDTO.Id, HttpContext.Session.GetString(SD.SessionToken));
                string content = resultItem.data.ToString();
                var item = JsonConvert.DeserializeObject<Blog>(content);
                if (file != null && file.Length > 0)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(item.imagePath))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, item.imagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Lưu ảnh mới
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Blog");
                    Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    blogDTO.ImagePath = $"Blog/{fileName}";
                }
                var result = await _blogService.Update(blogDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (result.success == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = result.errorList.FirstOrDefault();
                    return View(blogDTO);
                }
            }
            else
            {

                if (blogDTO.TagIDs == null || blogDTO.TagIDs.Count == 0)
                {
                    ViewBag.Message = "Vui long chon it nhat 1 tag loai";
                    return RedirectToAction("Update", new { id = blogDTO.Id });
                }
                ViewBag.message = "Vui long nhap thong tin";
                return RedirectToAction("Update", new { id = blogDTO.Id });
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var resultItemDeleted = await _blogService.GetOne(id, HttpContext.Session.GetString(SD.SessionToken));
            if (resultItemDeleted.data != null && resultItemDeleted.success == true)
            {
                string content = resultItemDeleted.data.ToString();
                var item = JsonConvert.DeserializeObject<Blog>(content);
                if (!string.IsNullOrEmpty(item.imagePath))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, item.imagePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                var result = await _blogService.Delete(id, HttpContext.Session.GetString(SD.SessionToken));
                if (result.success == true)
                {

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = result.errorList.FirstOrDefault();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Message = "Khong tim thay bai viet can xoa";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "admin,contributor")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BlogCreateDTO blogCreateDTO = new BlogCreateDTO();
            var categoryResponse = await _categoryService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            string categoryContent = categoryResponse.data.ToString();
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoryContent);
            ViewBag.Categories = categories;
            var tagResponse = await _tagService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            string tagContent = tagResponse.data.ToString();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(tagContent);
            ViewBag.AllTags = tags;
            return View(blogCreateDTO);
        }
        [Authorize(Roles = "admin,contributor")]
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateDTO blogCreateDTO, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (blogCreateDTO.TagIDs == null || blogCreateDTO.TagIDs.Count == 0)
                {
                    ViewBag.Message = "Vui long chon it nhat 1 tag loai";
                    return RedirectToAction("Create");
                }
                if (file != null && file.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Blog");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    blogCreateDTO.ImagePath = $"Blog/{uniqueFileName}";
                }
                var result = await _blogService.Create(blogCreateDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (result.success == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = result.errorList.FirstOrDefault();
                    return View(blogCreateDTO);
                }
            }
            else
            {
                ViewBag.Message = "Vui long nhap thong tin";
                return RedirectToAction("Create");
            }
        }
    }
}
        
    