using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Client.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IBlogService _blogService;
        public CategoryController( ICategoryService categoryService, IBlogService blogService)
        {
            _categoryService = categoryService;
            _blogService = blogService;
        }
        public IActionResult Index(int categoryID)
        {
            return View();
        }
    }
}
