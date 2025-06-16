using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Repository_Storage.Repo_Interface;
using System.Data;

namespace Blog_API.Repository_Storage
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Blog blog)
        {
            //_context.Update(blog);
            await Save();
        }
    }
}
