using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Client.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
