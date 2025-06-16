using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Repository_Storage.Repo_Interface;

namespace Blog_API.Repository_Storage
{
    public class BlogRatingRepository : Repository<BlogRating>, IBlogRatingRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(BlogRating blogRating)
        {
            _context.BlogRatings.Update(blogRating);
            await Save();
        }
    }
}
