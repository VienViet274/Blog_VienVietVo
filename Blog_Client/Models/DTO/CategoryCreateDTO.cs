using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
