using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class BlogUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<int> TagIDs { get; set; }
    }
}
