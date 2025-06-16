
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Client.Models
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Blog> blogs { get; set; }
    }
}
