using System.Net;

namespace Blog_API.Models
{
    public class APIResponse
    {
        public bool Success { get; set; }
        public List<string> ErrorList { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object data { get; set; }
    }
}
