using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog_Client.Controllers
{
    public class CategoryAdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryAdminController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var CategoryResult = await _categoryService.GetAll(HttpContext.Session.GetString(SD.SessionToken), null);
            string Categorystring = CategoryResult.data.ToString();
            var categorylist = JsonConvert.DeserializeObject<List<Category>>(Categorystring);
            if (categorylist != null && categorylist.Count > 0)
            {
                return View(categorylist);
            }
            return View();
        }
        public async Task<IActionResult> Create()
        {
            CategoryCreateDTO categoryCreateDTO = new CategoryCreateDTO();
            return View(categoryCreateDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO category)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.Create(category, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to create category.");
            }
            return View(category);
        }
        public async Task<IActionResult> Update(int id)
        {
            var response = await _categoryService.GetOne(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.success)
            {
                string categoryString = response.data.ToString();
                var category = JsonConvert.DeserializeObject<Category>(categoryString);
                CategoryUpdateDTO categoryUpdateDTO = new CategoryUpdateDTO
                {
                    Id = category.id,
                    Name = category.name
                };
                return View(categoryUpdateDTO);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDTO category)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.Update(category, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to update category.");
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.Delete(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
