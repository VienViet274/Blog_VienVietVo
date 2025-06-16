using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
