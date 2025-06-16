using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class TagCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public List<int>? BlogIDs { get; set; }
    }
}
