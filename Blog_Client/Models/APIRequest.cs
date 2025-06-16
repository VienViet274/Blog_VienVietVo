using Blog_Utilities;

namespace Blog_Client.Models
{
    public class APIRequest
    {
        public string URL { get; set; }
        public APIType APIType { get; set; }
        public object body { get; set; }
        public string token { get; set; }
    }
}
