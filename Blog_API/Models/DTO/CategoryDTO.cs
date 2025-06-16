using System.ComponentModel.DataAnnotations;

namespace Blog_API.Models.DTO
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
