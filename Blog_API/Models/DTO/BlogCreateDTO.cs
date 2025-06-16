using System.ComponentModel.DataAnnotations;

namespace Blog_API.Models.DTO
{
    public class BlogCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public List<int> TagIDs { get; set; }
    }
}
