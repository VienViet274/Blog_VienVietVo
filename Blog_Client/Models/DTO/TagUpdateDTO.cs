using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class TagUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MinLength(1)]
        public List<int> BlogIDs { get; set; }

        
        // This property is used to track the original state of the tag for concurrency checks
        }
}
