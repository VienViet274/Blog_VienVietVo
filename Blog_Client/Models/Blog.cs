using Blog_Client.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Client.Models
{
    public class Blog
    {

        public int id { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
        public string imagePath { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
        public List<Tag> tags { get; set; }
        public List<Comment> comments { get; set; }

    }
}
