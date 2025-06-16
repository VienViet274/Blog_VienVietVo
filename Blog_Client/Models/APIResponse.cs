using System.Net;

namespace   Blog_Client.Models
{
    public class APIResponse
    {
        public bool success { get; set; }
        public List<string> errorList { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public object data { get; set; }
    }
}
