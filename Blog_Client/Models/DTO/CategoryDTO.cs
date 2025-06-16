using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
