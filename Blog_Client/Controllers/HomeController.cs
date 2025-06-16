
using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;



namespace Blog_Client.Controllers
{
    [Authorize(Roles = "admin,contributor,reader")]
    public class HomeController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        public HomeController(IBlogService blogService, ICommentService commentService)
        {
            _blogService = blogService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index([FromQuery] string Search)
        {
            var tets = await _blogService.GetAll(HttpContext.Session.GetString(SD.SessionToken), Search);
            var blogs = tets.data;
            string content = blogs.ToString();
            List<Blog> list = new List<Blog>();
            list = JsonConvert.DeserializeObject<List<Blog>>(content);
            return View(list);
        }

        
        public async Task<IActionResult> Post(int id)
        {
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //string role = User.FindFirst(ClaimTypes.Role)?.Value;
            var tets = await _blogService.GetOne(id, HttpContext.Session.GetString(SD.SessionToken));
            if(tets == null || tets.data == null)
            {
                return NotFound();
            }
            var blog = tets.data;
            string blogString = blog.ToString();
            var item = JsonConvert.DeserializeObject<Blog>(blogString);
            if(item == null)
            {
                return NotFound();
            }
            item.comments =  item.comments.Where(x => x.Approved == true).ToList();
            //var check =  item.comments.Select(x => x.Approved == true).ToList();

            return View(item);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Contact()
        {
            return View();
        }
        public async Task<IActionResult> About()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string Message)
        {
            CommentDTO commentDTO = new CommentDTO
            {
                BlogID = id,
                message = Message
            };
            var response = await _commentService.AddComment(commentDTO, HttpContext.Session.GetString(SD.SessionToken));
               return RedirectToAction("Post", new { id = id });
        }
    }
}
