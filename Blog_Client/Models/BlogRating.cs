using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Client.Models
{
    public class BlogRating
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Blog))]
        public int BlogID { get; set; }
        public Blog Blog { get; set; }
        public double Rating { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
