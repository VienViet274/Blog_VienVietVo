using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog_Client.Controllers
{
    public class CommentAdminController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        public CommentAdminController(IBlogService blogService, ICommentService commentService)
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
        public async Task<IActionResult> Manage(int id)
        {
            var tets = await _blogService.GetOne(id, HttpContext.Session.GetString(SD.SessionToken));
            var blog = tets.data;
            string blogString = blog.ToString();
            var item = JsonConvert.DeserializeObject<Blog>(blogString);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Approve( int commentId)
        {
            var response = await _commentService.ApproveComment(commentId, HttpContext.Session.GetString(SD.SessionToken));
            if (response.success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Manage", new { id = commentId });  
            }
        }
    }
}