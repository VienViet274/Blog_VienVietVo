using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class TagDTO
    {
        [Required]
        public string Name { get; set; }
        [MinLength(1)]
        public List<int> BlogIDs { get; set; }
    }
}
