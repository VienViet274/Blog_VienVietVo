using Blog_API.Data;
using Blog_API.Models;
using Blog_API.Repository_Storage.Repo_Interface;

namespace Blog_API.Repository_Storage
{
    public class CategoryRepository:  Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Category category)
        {
            _context.Update(category);
            await Save();
        }
    }
}
